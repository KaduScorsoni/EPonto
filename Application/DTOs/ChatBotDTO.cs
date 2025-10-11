namespace Application.DTOs
{
    public class ChatBotDTO : BaseDTO
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public string Response { get; set; }
        public string CurrentCommand { get; set; }
    }

    public class BaseDTO
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
    }
}
