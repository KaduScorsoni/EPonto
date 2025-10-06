// ...existing code...
using Domain.Entities.Perfil;

public interface IPerfilRepository
{
    Task<int> CadastrarPerfil(PerfilModel param);
    Task<int> CadastrarVinculoPerfilUsuario(int idUsuario, int idPerfil);
    Task<List<PerfilModel>> ListarPerfis();
    Task<PerfilModel> ListarPerfil(int idPerfil);
    Task<int> EditarPerfil(PerfilModel param);
    Task<int> RemoverPerfil(int idPerfil);
    Task<bool> VerificaPerfilEmUso(int idPerfil);
    Task<int> RemoveVinculoPerfilUsuario(int idUsuario);
}
