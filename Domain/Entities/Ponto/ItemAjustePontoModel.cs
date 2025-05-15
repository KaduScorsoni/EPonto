using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Ponto
{
    public class ItemAjustePontoModel
    {
        public int IdItem { get; set; }
        public int IdSolicitacao { get; set; }
        public DateTime? HoraRegistro { get; set; }
        public int IdTipoRegistroPonto { get; set; }
    }
}
