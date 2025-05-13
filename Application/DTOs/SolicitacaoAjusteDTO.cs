using Domain.Entities;
using Domain.Entities.Ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SolicitacaoAjusteDTO
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public SolicitacaoAjustePontoModel SolicitacaoAjustePontoModel { get; set; }
        public IEnumerable<SolicitacaoAjustePontoModel> Solicitacoes { get; set; }
    }
}
