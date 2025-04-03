using Domain.Entities.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ILoginRepository
    {
        public bool BuscaUsuarioNoSistema(LoginModel dadosInformados);
    }
}
