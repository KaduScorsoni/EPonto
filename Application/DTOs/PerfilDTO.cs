namespace Application.DTOs
{
    public class PerfilDTO
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public List<Domain.Entities.Perfil.PerfilModel> ListaPerfis { get; set; }
    }
}
