using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ContratoUsuarioDTO
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public ContratoUsuarioModel Contrato { get; set; }
    }
}
