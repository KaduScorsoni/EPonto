using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enum
{
    public class EnumGerais
    {
        public enum SituacaoFeriado
        {
            [Description("Feriado Integral")]
            FeriadoIntegral = 1,
            [Description("Feriado Meio Periodo")]
            FeriadoMeioPeriodo = 2
        }
    }
}
