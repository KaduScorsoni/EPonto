using Application.DTOs;
using Domain.Entities.WhatsApp;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IChatBotService
    {
        Task<ResultadoDTO> ProcessMessage(string phoneNumber, string message);
        Task<ChatSessionModel> GetOrCreateSession(string phoneNumber);
        Task<ResultadoDTO> ProcessVacationCommand(ChatSessionModel session, string message);
        Task<string> GetHelpMessage();
    }
}
