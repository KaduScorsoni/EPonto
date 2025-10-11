using Domain.Entities.WhatsApp;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IWhatsAppRepository
    {
        Task<WhatsAppConfigModel> GetActiveConfig();
        Task SaveConfig(WhatsAppConfigModel config);
        Task<ChatSessionModel> GetActiveSession(string phoneNumber);
        Task CreateSession(ChatSessionModel session);
        Task UpdateSession(ChatSessionModel session);
    }
}
