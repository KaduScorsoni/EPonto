using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Calendario
{
    public class CalendarioModel
    {
        public DateTime DatEvento { get; set; }
        public string DscEvento { get; set; }
        public decimal SaldoHorasDiario { get; set; }
        public int TipoEvento { get; set; }
    }
}
