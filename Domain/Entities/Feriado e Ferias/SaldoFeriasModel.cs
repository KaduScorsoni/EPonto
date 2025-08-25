using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Feriado_e_Ferias
{
    public class SaldoFeriasModel
    {
        public int? IdUsuario { get; set; }
        public int QtdSaldo { get; set; }
        public string NomeUsuario { get; set; }
    }
}
