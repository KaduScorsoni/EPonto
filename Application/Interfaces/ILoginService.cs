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
        public string HashPassword(string senha);
        public bool VerifyPassword(string senha, string senhaHash);
        public string GenerateToken(string idUsuario, string email, string senhaHash, int expireMinutes = 60);
    }
}
