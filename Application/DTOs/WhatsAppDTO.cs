using Domain.Entities.WhatsApp;

namespace Application.DTOs
{
    public class WhatsAppDTO : BaseDTO
    {
        public WhatsAppConfigModel Config { get; set; }
    }

    public class ChatBotResponseDTO : BaseDTO
    {
        public string Response { get; set; }
        public string Command { get; set; }
        public object Data { get; set; }
    }
}
