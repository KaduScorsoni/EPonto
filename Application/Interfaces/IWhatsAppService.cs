using Application.DTOs;
using Domain.Entities.WhatsApp;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWhatsAppService
    {
        Task<ResultadoDTO> SendMessage(string phoneNumber, string message);
        Task<ResultadoDTO> ProcessWebhook(WhatsAppWebhookModel webhook);
        Task<WhatsAppConfigModel> GetActiveConfig();
        Task<ResultadoDTO> SaveConfig(WhatsAppConfigModel config);
        Task<bool> VerifyWebhook(string mode, string token, string challenge);
    }
}
