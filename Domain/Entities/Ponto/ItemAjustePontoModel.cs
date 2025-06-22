using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities.Ponto
{
    public class ItemAjustePontoModel
    {
        [JsonIgnore]
        public int IdItem { get; set; }
        [JsonIgnore]
        public int IdSolicitacao { get; set; }
        public DateTime? HoraRegistro { get; set; }
        public int IdTipoRegistroPonto { get; set; }
        public string Localizacao { get; set; }
    }
}
