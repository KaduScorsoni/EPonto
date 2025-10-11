using Application.DTOs;
using Application.Interfaces;
using Data.Interfaces;
using Domain.Entities.WhatsApp;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly IWhatsAppRepository _whatsAppRepository;
        private readonly IChatBotService _chatBotService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WhatsAppService(
            IWhatsAppRepository whatsAppRepository, 
            IChatBotService chatBotService,
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _whatsAppRepository = whatsAppRepository;
            _chatBotService = chatBotService;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<ResultadoDTO> SendMessage(string phoneNumber, string message)
        {
            try
            {
                var config = await GetActiveConfig();
                if (config == null)
                    return new ResultadoDTO { Sucesso = false, Mensagem = "Configuração do WhatsApp não encontrada." };

                var requestBody = new
                {
                    messaging_product = "whatsapp",
                    to = phoneNumber,
                    type = "text",
                    text = new { body = message }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.AccessToken}");

                var url = $"https://graph.facebook.com/v18.0/{config.PhoneNumberId}/messages";
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return new ResultadoDTO { Sucesso = true, Mensagem = "Mensagem enviada com sucesso." };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ResultadoDTO { Sucesso = false, Mensagem = $"Erro ao enviar mensagem: {errorContent}" };
                }
            }
            catch (Exception ex)
            {
                return new ResultadoDTO { Sucesso = false, Mensagem = $"Erro: {ex.Message}" };
            }
        }

        public async Task<ResultadoDTO> ProcessWebhook(WhatsAppWebhookModel webhook)
        {
            try
            {
                if (webhook?.Entry == null) return new ResultadoDTO { Sucesso = true, Mensagem = "Webhook vazio." };

                foreach (var entry in webhook.Entry)
                {
                    foreach (var change in entry.Changes)
                    {
                        if (change.Value?.Messages != null)
                        {
                            foreach (var message in change.Value.Messages)
                            {
                                if (message.Type == "text")
                                {
                                    await _chatBotService.ProcessMessage(message.From, message.Text);
                                }
                            }
                        }
                    }
                }

                return new ResultadoDTO { Sucesso = true, Mensagem = "Webhook processado com sucesso." };
            }
            catch (Exception ex)
            {
                return new ResultadoDTO { Sucesso = false, Mensagem = $"Erro ao processar webhook: {ex.Message}" };
            }
        }

        public async Task<WhatsAppConfigModel> GetActiveConfig()
        {
            return await _whatsAppRepository.GetActiveConfig();
        }

        public async Task<ResultadoDTO> SaveConfig(WhatsAppConfigModel config)
        {
            try
            {
                await _whatsAppRepository.SaveConfig(config);
                return new ResultadoDTO { Sucesso = true, Mensagem = "Configuração salva com sucesso." };
            }
            catch (Exception ex)
            {
                return new ResultadoDTO { Sucesso = false, Mensagem = $"Erro ao salvar configuração: {ex.Message}" };
            }
        }

        public async Task<bool> VerifyWebhook(string mode, string token, string challenge)
        {
            var verifyToken = _configuration["WhatsApp:VerifyToken"];
            
            if (mode == "subscribe" && token == verifyToken)
            {
                return true;
            }
            
            return false;
        }
    }
}
