using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities.Ponto
{
    public class SolicitacaoAjustePontoModel
    {
        public int IdSolicitacao { get; set; }
        public int IdSolicitante { get; set; }
        public string Justificativa { get; set; }
        public StatusAlteracaoPonto StatusSolicitacao { get; set; }
        [JsonIgnore]
        public DateTime DataSolicitacao { get; set; }
        public DateTime DataRegistroAlteracao { get; set; }
        [JsonIgnore]
        public DateTime? DataResposta { get; set; }
        [JsonIgnore]
        public DateTime? HoraRegistro { get; set; }
        public List<ItemAjustePontoModel> Itens { get; set; }
    }
}
