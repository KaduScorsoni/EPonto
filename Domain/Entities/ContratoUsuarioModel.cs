using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ContratoUsuarioModel
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public int IndAtivo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public int Telefone { get; set; }
        public string? FotoPerfil { get; set; }
        public string NomeCargo { get; set; }
        public int Salario { get; set; }
        public int QtdHorasDiarias { get; set; }
    }
}
