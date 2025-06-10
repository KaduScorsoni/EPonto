using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Feriado_e_Ferias
{
    public class ResultadoFeriasModel
    {
        public string DscFerias { get; set; }
        public DateTime DatIncioFerias { get; set; }
        public DateTime DatFimFerias { get; set; }
        public int? IdUsuario { get; set; }
        public long IdFerias { get; set; }
    }
}
