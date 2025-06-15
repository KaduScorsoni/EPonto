using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Entities.Enum.EnumGerais;

namespace Domain.Entities.Feriado_e_Ferias
{
    public class ResultadoFeriadoModel
    {
        public string DscFeriado { get; set; }
        public DateTime DatFeriado { get; set; }
        public string DscTipoFeriado { get; set; }
        public long IdFeriado { get; set; }
    }
}
