using Domain.Entities.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AutenticarDTO
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public long IdUsuario { get; set; }
        public string Token { get; set; }
    }
}
