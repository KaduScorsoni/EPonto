using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class FeriadoDTO
    {
        public string Mensagem { get; set; }
        public bool Sucesso { get; set; }
        public List<FeriadoModel> ListaFeriados { get; set; }
    }
}
