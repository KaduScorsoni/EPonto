using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Feriado_e_Ferias
{
    public class ResultadoSolicitacaoFeriasModel
    {
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public int IdSolicFerias { get; set; }
        public int IndSituacao { get; set; }
        public string DscObservacao { get; set; }
        public DateTime DatInicioFerias { get; set; }
        public DateTime DatFimFerias { get; set; }
        public DateTime DatSolicitacaoFerias { get; set; }
    }
}
