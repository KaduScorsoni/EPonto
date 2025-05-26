using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class JornadaTrabalhoModel
    {
        public int IdJornada { get; set; }
        public string NomeJornada { get; set; }
        public int QtdHorasMensais { get; set; }
        [JsonIgnore]
        public int IndAtivo { get; set; }
    }
}
