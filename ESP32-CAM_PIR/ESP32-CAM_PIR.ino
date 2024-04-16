#include <SPIFFS.h>

#include "WiFi.h"
#include "esp_camera.h"
#include "esp_timer.h"
#include "img_converters.h"
#include "Arduino.h"
#include "soc/soc.h"
#include "soc/rtc_cntl_reg.h"
#include "driver/rtc_io.h"

#include <PubSubClient.h>
#include <base64.h>

//Credenciais de rede vão aqui
const char* ssid = "NOME_DA_SUA_REDE";
const char* senha = "SENHA_DA_SUA_REDE";

//Configuração do broker MQTT
const char* mqtt_servidor = SERVIDOR;
const int mqtt_porta = PORTA;
const char* mqtt_usuario = "TOKEN_MQTT";
const char* mqtt_senha = "SENHA_MQTT";

//Nome do tópico
const char* mqtt_topico = "esp32/photo_1";

framesize_t resolution_ = FRAMESIZE_QVGA;

#define SLEEP_DELAY 0

//Configuração dos pinos
#define PWDN_GPIO_NUM     32
#define RESET_GPIO_NUM    -1
#define XCLK_GPIO_NUM      0
#define SIOD_GPIO_NUM     26
#define SIOC_GPIO_NUM     27
#define Y9_GPIO_NUM       35
#define Y8_GPIO_NUM       34
#define Y7_GPIO_NUM       39
#define Y6_GPIO_NUM       36
#define Y5_GPIO_NUM       21
#define Y4_GPIO_NUM       19
#define Y3_GPIO_NUM       18
#define Y2_GPIO_NUM        5
#define VSYNC_GPIO_NUM    25
#define HREF_GPIO_NUM     23
#define PCLK_GPIO_NUM     22

WiFiClient mqttClient;
PubSubClient client(mqttClient);

const int LED_BUILTIN = 4;

void setup_camera(){
  // Desligar 'brownout detector'
  WRITE_PERI_REG(RTC_CNTL_BROWN_OUT_REG, 0);

  // OV2640
  camera_config_t config;
  config.ledc_channel = LEDC_CHANNEL_0;
  config.ledc_timer = LEDC_TIMER_0;
  config.pin_d0 = Y2_GPIO_NUM;
  config.pin_d1 = Y3_GPIO_NUM;
  config.pin_d2 = Y4_GPIO_NUM;
  config.pin_d3 = Y5_GPIO_NUM;
  config.pin_d4 = Y6_GPIO_NUM;
  config.pin_d5 = Y7_GPIO_NUM;
  config.pin_d6 = Y8_GPIO_NUM;
  config.pin_d7 = Y9_GPIO_NUM;
  config.pin_xclk = XCLK_GPIO_NUM;
  config.pin_pclk = PCLK_GPIO_NUM;
  config.pin_vsync = VSYNC_GPIO_NUM;
  config.pin_href = HREF_GPIO_NUM;
  config.pin_sscb_sda = SIOD_GPIO_NUM;
  config.pin_sscb_scl = SIOC_GPIO_NUM;
  config.pin_pwdn = PWDN_GPIO_NUM;
  config.pin_reset = RESET_GPIO_NUM;
  config.xclk_freq_hz = 20000000;
  config.pixel_format = PIXFORMAT_JPEG;

  if (psramFound()) {
    config.frame_size = resolution_  ;
    config.jpeg_quality = 10;
    config.fb_count = 1;
  } else {
    config.frame_size = FRAMESIZE_SVGA;
    config.jpeg_quality = 12;
    config.fb_count = 2;
  }
  // Iniciar câmera
  esp_err_t err = esp_camera_init(&config);
  if (err != ESP_OK) {
    Serial.printf("Erro 0x%x ao iniciar camera\n", err);
    ESP.restart();
  }
}

void publicarFoto(String data) {
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  boolean Status = client.publish_P( mqtt_topico, (const uint8_t*) data.c_str(), data.length(), true);
  Serial.println(String(Status ? "Sucesso!" : "Erro") );
}

void capturarFoto(){
  camera_fb_t * fb = NULL;
  uint8_t* _jpg_buf = NULL;
  esp_err_t res = ESP_OK;
  size_t frame_size = 0;
  Serial.print("Capturando Imagem...");

  digitalWrite(LED_BUILTIN, HIGH);   // liga o LED
  delay(1000);                       // espera 1 segundo
  fb = esp_camera_fb_get();
  digitalWrite(LED_BUILTIN, LOW);    // desliga o LED
  delay(1000);                       // espera 1 segundo
  if (!fb) {
    Serial.println("Falha na captura");
    res = ESP_FAIL;
  } else {
    Serial.println("Imagem capturada!");
    Serial.println(String("Tamanho da imagem...") + String(fb->len));
    if (fb->format != PIXFORMAT_JPEG) {
      Serial.println("Comprimindo imagem");
      bool jpeg_converted = frame2jpg(fb, 80, &_jpg_buf, &frame_size);
      esp_camera_fb_return(fb);
      fb = NULL;
      if (!jpeg_converted) {
        Serial.println("Falha ao comprimir JPEG");
        res = ESP_FAIL;
      }
    } else {
      frame_size = fb->len;
      _jpg_buf = fb->buf;
      Serial.print("Publicando foto...");

      publicarFoto(base64::encode(_jpg_buf, fb->len));
      Serial.println("Publicado com sucesso!");
      esp_camera_fb_return(fb);
    }
  }
  if (res != ESP_OK) {
    return;
  }
}

void setup_wifi(){
  delay(10);
  // Conectando à rede WiFi
  Serial.println();
  Serial.print("Conectando a ");
  Serial.println(ssid);

  WiFi.begin(ssid, senha);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi conectada");
  Serial.print("IP : ");
  Serial.println(WiFi.localIP());
}

void reconectar() {
  while (!client.connected()) {
    Serial.print("Conectando ao MQTT ...");
    if (client.connect("ESP32Client", mqtt_usuario, mqtt_senha)) {
      Serial.println("conectado");
    } else {
      Serial.print("falha, rc=");
      Serial.print(client.state());
      Serial.println(" tentando novamente em 5 segundos");
      delay(5000);
    }
  }
}


void setup() {
  Serial.begin(115200);

  setup_camera();
  pinMode(LED_BUILTIN, OUTPUT);
  setup_wifi();
  client.setServer(mqtt_servidor, mqtt_porta);
  if (!SPIFFS.begin(true)) {
    Serial.println("Erro ao conectar ao SPIFFS");
    return;
  }
}

void loop() {
  Serial.println("PSRAM encontrada: " + String(psramFound()));

  if (!client.connected()) {
    reconectar();
  }
  if (client.connected()) {
    capturarFoto();
  }
  client.loop();

  Serial.println("Iniciando sleep...");
  if (SLEEP_DELAY == 0) {
    Serial.println("Delay setado para zero, aguardando PIR");
    esp_sleep_enable_ext0_wakeup(GPIO_NUM_13, 0);
    delay(1000);
    esp_deep_sleep_start();
  }

}
