using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SaldoDiarioBancoHorasModel
    {
        public int IdSaldoDiarioBancoHoras { get; set; }
        public int IdUsuario { get; set; }
        public TimeSpan SaldoDiario { get; set; }
        public DateTime DataReferencia { get; set; }
    }
}
