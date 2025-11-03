using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Relatorios
{
    public class RelHorasExtrasModel
    {
        public string NomeUsuario { get; set; }
        public string SaldoHoras { get; set; }
        public string Cargo { get; set; }
        public string JornadaTrabalho { get; set; }
        public string HorasTrabalhadasDiarias { get; set; }
    }
}
