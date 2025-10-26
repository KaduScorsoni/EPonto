using Domain.Entities.Login;
using Domain.Entities.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ILoginRepository
    {
        Task<LoginAuxiliarModel> BuscaUsuarioNoSistema(string email);
        Task<bool> InsereRegistroLogin(long IdUsuario, string token);
        Task<bool> VerificaPerfilAdministrador(int IdPerfil, long IdUsuario);
        Task<bool> SalvaCodigoRecuperacao(int codigo, string email);
        Task<int> BuscaCodigoEmail(string email);
        Task<bool> SalvaAlteracaoSenha(string senha, string email);
        Task<List<IdDescricaoPerfilModel>> RetornaPerfilUsuario(long idUsuario);
    }
}
