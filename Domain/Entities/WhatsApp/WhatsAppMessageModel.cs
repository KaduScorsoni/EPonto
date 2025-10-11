using System;

namespace Domain.Entities.WhatsApp
{
    public class WhatsAppMessageModel
    {
        public string From { get; set; }
        public string Id { get; set; }
        public string Timestamp { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
    }

    public class WhatsAppWebhookModel
    {
        public string Object { get; set; }
        public WhatsAppEntry[] Entry { get; set; }
    }

    public class WhatsAppEntry
    {
        public string Id { get; set; }
        public WhatsAppChange[] Changes { get; set; }
    }

    public class WhatsAppChange
    {
        public WhatsAppValue Value { get; set; }
        public string Field { get; set; }
    }

    public class WhatsAppValue
    {
        public string MessagingProduct { get; set; }
        public WhatsAppMetadata Metadata { get; set; }
        public WhatsAppContact[] Contacts { get; set; }
        public WhatsAppMessageModel[] Messages { get; set; }
    }

    public class WhatsAppMetadata
    {
        public string DisplayPhoneNumber { get; set; }
        public string PhoneNumberId { get; set; }
    }

    public class WhatsAppContact
    {
        public WhatsAppProfile Profile { get; set; }
        public string WaId { get; set; }
    }

    public class WhatsAppProfile
    {
        public string Name { get; set; }
    }
}
