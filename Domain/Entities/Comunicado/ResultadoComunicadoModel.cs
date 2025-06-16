using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Comunicado
{
    public class ResultadoComunicadoModel
    {
        public int IdComunicado { get; set; }
        public string DscComunicado { get; set; }
        public string TituloComunicado { get; set; }
        public DateTime DatInicioExibicao { get; set; }
        public DateTime DatFimExibicao { get; set; }
    }
}
