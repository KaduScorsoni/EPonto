using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UsuarioDTO
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public UsuarioModel Usuario { get; set; }
        public IEnumerable<UsuarioModel> Usuarios { get; set; }
    }
}
