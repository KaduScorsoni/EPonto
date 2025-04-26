using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RegistroPontoModel
    {
        public int Id_Registro { get; set; }
        public int Id_Usuario { get; set; }
        public DateTime Hora_Registro { get; set; }
        public DateTime Data_Registro { get; set; }
        public int Id_Tipo_Registro_Ponto { get; set; }
    }
}
