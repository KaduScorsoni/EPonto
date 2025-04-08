using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UsuarioModel
    {
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }
        [Column("NOME")]
        public string Nome { get; set; }
        [Column("DATA_NASCIMENTO")]
        public DateTime DataNascimento { get; set; }
        [Column("SENHA")]
        public string Senha { get; set; }
        [Column("EMAIL")]
        public string Email { get; set; }
        [Column("TELEFONE")]
        public int Telefone { get; set; }
        [Column("ID_CARGO")]
        public int IdCargo { get; set; }
        [Column("ID_JORNADA")]
        public int IdJornada { get; set; }
    }
}
