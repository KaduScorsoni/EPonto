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
        public LoginDTO RealizarLogin(LoginModel dadosInformados);
    }
}
