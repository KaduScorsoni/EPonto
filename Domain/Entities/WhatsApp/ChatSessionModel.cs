using System;

namespace Domain.Entities.WhatsApp
{
    public class ChatSessionModel
    {
        public int IdSession { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public int? IdUsuario { get; set; }
        public string CurrentCommand { get; set; }
        public string SessionData { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataUltimaInteracao { get; set; }
        public bool IsActive { get; set; }
    }
}
