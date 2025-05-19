using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CargoDTO
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public CargoModel Cargo { get; set; }
        public IEnumerable<CargoModel> Cargos { get; set; }
    }
}
