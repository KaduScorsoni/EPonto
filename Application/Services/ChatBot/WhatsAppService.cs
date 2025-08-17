using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ChatBot
{
    internal class WhatsAppService
    {
        private readonly string _token = ""; //"SEU_TOKEN_PERMANENTE_AQUI";
        private readonly string _phoneNumberId = "689642657574980"; //"SEU_PHONE_NUMBER_ID";

        public async Task SendMessageAsync(string to, string message)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

            var json = $@"{{
                ""messaging_product"": ""whatsapp"",
                ""to"": ""{to}"",
                ""type"": ""text"",
                ""text"": {{ ""body"": ""{message}"" }}
            }}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                $"https://graph.facebook.com/v21.0/{_phoneNumberId}/messages",
                content
            );

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }

        //Abaixo é um exemplo de como utilizar o método criado acima
        //var ws = new WhatsAppService();
        //await ws.SendMessageAsync("5581999999999", "Olá, esta é uma mensagem automática!");

    }
}
