# ESP32-CAM

## O projeto
O projeto se trata de um sistema que utiliza um ESP32 em conjunto com um sensor de presença PIR para notificar o usuário de movimentação em um local específico.

## Componentes
Para implementar a solução foram utilizados:
- ESP32-CAM
- Arduino Uno (utilizado apenas para transferência do código para o ESP32)
- Sensor de movimento
- Resistor 1kΩ
- Resistor 10kΩ
- Transistor 2n2222a

Os componentes foram conectados conforme o esquemático:
![esquemático](/imgs/PIR_ESP32_scheme.png)

## Funcionamento
- O ESP32-CAM se conecta à rede utilizando as credenciais
```
//Credenciais de rede vão aqui
const char* ssid = "NOME_DA_SUA_REDE";
const char* senha = "SENHA_DA_SUA_REDE";

WiFi.begin(ssid, senha);
```

- A conexão MQTT é estabelecida
```
//Configuração do broker MQTT
const char* mqtt_servidor = SERVIDOR;
const int mqtt_porta = PORTA;
const char* mqtt_usuario = "TOKEN_MQTT";
const char* mqtt_senha = "SENHA_MQTT";

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
```

- O dispositivo fica então no aguardo de ser "acordado" pelo sensor quando é detectado movimento
```
esp_sleep_enable_ext0_wakeup(GPIO_NUM_13, 0);
```

- Quando há detecção de movimento, um registro fotográfico é realizado e publicado
```
void publicarFoto(String data) {
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  boolean Status = client.publish_P( mqtt_topico, (const uint8_t*) data.c_str(), data.length(), true);
  Serial.println(String(Status ? "Sucesso!" : "Erro") );
}
```

![painel](/imgs/painel.png)
