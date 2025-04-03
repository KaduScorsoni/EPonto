using Data.Interfaces;
using Domain.Entities.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        public bool BuscaUsuarioNoSistema(LoginModel dadosInformados)
        {
            throw new NotImplementedException();
        }
    }
}
