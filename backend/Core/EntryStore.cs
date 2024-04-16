using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace backend
{
    public class EntryStore
    {
        HttpClient cliente = new HttpClient();

        public EntryStore ()
        {
            cliente.BaseAddress = new Uri("http://localhost:5000/");

            cliente.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("json"));
        }

        public async Task<Entry> GetEntryAsync()
        {
            HttpResponseMessage response = await cliente.GetAsync("https://flespi.io/mqtt/messages/esp32%2Fphoto_1?fields=&data=%7B%7D");
            
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Entry>(data);
            }

            return new Entry();
        }
    }
}