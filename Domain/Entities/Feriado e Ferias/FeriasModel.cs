using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Domain.Entities.Enum.EnumGerais;

namespace Domain.Entities
{
    public class FeriasModel
    {
        public string DscFerias { get; set; }
        public DateTime DatIncioFerias { get; set; }
        public DateTime DatFimFerias { get; set; }
        public int? IdUsuario { get; set; }
        [JsonIgnore]
        public long IdFerias { get; set; }
    }
}
