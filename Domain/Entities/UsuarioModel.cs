using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public long Telefone { get; set; }
        public int IdCargo { get; set; }
        public int IdJornada { get; set; }
        public int IndAtivo { get; set; }
        public string? FotoPerfil { get; set; }
        public int? IdChefe { get; set; }
        public UsuarioModel? Chefe { get; set; } // navegação p/ chefe
        public ICollection<UsuarioModel> Subordinados { get; set; } =  new List<UsuarioModel>(); // navegação p/ subordinados
        public int Nivel { get; set; }
    }
}
