using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities.Feriado_e_Ferias
{
    public class SolicitacaoFeriasModel
    {
        public int IdUsuario { get; set; }
        [JsonIgnore]
        public int IdSolicFerias { get; set; }
        public string DscObservacao { get; set; }
        public DateTime DatInicioFerias { get; set; }
        public DateTime DatFimFerias { get; set; }
        public DateTime DatInicio { get; set; }
    }
}
