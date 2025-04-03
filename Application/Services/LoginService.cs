using Application.DTOs;
using Application.Interfaces;
using Data.Interfaces;
using Domain.Entities.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public LoginDTO RealizarLogin(LoginModel dadosInformados)
        {
            

            throw new NotImplementedException();
        }
    }

}
