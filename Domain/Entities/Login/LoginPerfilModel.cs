using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Login
{
    public class LoginPerfilModel
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public int IdPerfil { get; set; }
    }
}
