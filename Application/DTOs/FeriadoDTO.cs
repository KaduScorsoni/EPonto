using Domain.Entities;
using Domain.Entities.Feriado_e_Ferias;
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
        public List<ResultadoFeriadoModel> ListaFeriados { get; set; }
    }
}
