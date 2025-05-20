using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RegistroPontoModel
    {
        [JsonIgnore]
        public int IdRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime HoraRegistro { get; set; }
        public DateTime DataRegistro { get; set; }
        public int IdTipoRegistroPonto { get; set; }
    }
}
