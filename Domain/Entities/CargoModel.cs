using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CargoModel
    {
        public int IdCargo { get; set; }
        public string NomeCargo { get; set; }
        public string Salario { get; set; }
        [JsonIgnore]
        public int IndAtivo { get; set; }
    }
}
