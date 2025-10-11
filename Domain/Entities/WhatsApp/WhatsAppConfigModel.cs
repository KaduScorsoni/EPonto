using System;

namespace Domain.Entities.WhatsApp
{
    public class WhatsAppConfigModel
    {
        public int IdConfig { get; set; }
        public string PhoneNumberId { get; set; }
        public string AccessToken { get; set; }
        public string VerifyToken { get; set; }
        public string BusinessAccountId { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
