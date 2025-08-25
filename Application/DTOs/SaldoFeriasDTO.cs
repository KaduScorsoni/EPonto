using Domain.Entities.Feriado_e_Ferias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SaldoFeriasDTO
    {
        public bool Sucesso { get; set; }
        public String Mensagem { get; set; }
        public List<SaldoFeriasModel> ListaSaldoFerias { get; set; }
    }
}
