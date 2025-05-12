using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Ponto
{
    public class SolicitacaoAjustePontoModel
    {
        public int IdSolicitacao { get; set; }
        public string Justificativa { get; set; }
        public StatusAlteracaoPonto StatusSolicitacao { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataResposta { get; set; }
        public DateTime? HoraRegistro { get; set; }
        public int? IdGestorResponsavel { get; set; }
        public List<ItemAjustePontoModel> Itens { get; set; }
    }
}
