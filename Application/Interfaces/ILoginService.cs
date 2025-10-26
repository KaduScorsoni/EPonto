using Application.DTOs;
using Domain.Entities.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILoginService
    {
        Task<LoginDTO> RealizarLogin(LoginModel dadosInformados);
        Task<AutenticarDTO> AutenticarPerfil(LoginPerfilModel dadosInformados);
        Task<List<IdDescricaoPerfilModel>> RetornaPerfilUsuario(long idPerfil);
        Task<bool> RecuperarSenha(string email);
        Task<bool> ValidaCodigoRecuperacao(int codigo, string email);
        Task<bool> AlteraSenhaLogin(string senha, string email);
        public string HashPassword(string senha);
        public bool VerifyPassword(string senha, string senhaHash);
        public string GenerateToken(string idUsuario, string email, string senhaHash, bool isAdmin,  int expireMinutes = 60);
    }
}
