using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BancoHorasModel
    {
        public int IdBancoHoras { get; set; }
        public int IdUsuario { get; set; }
        public TimeSpan HorasTrabalhadas { get; set; }
        public TimeSpan Saldo { get; set; }
    }
}
