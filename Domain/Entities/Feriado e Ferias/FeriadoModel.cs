using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Domain.Entities.Enum.EnumGerais;

namespace Domain.Entities
{
    public class FeriadoModel
    {
        public string DscFeriado { get; set; }
        public DateTime DatFeriado { get; set; }
        public SituacaoFeriado IndTipoFeriado { get; set; }
        [JsonIgnore] 
        public long IdFeriado { get; set; }
        
    }
}
