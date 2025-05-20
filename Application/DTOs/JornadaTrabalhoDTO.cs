using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class JornadaTrabalhoDTO
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public JornadaTrabalhoModel Jornada { get; set; }
        public IEnumerable<JornadaTrabalhoModel> Jornadas { get; set; }
    }
}

