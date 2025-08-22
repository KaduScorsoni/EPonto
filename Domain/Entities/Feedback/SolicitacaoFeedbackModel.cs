using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Feedback
{
    public class SolicitacaoFeedbackModel
    {
        public int IdSolicitacaoFeedback { get; set; }
        public int IdUsuarioSolicitacao { get; set; }
        public string? NomeUsuarioSolicitacao { get; set; }
        public int IdResponsavelFeedback { get; set; }
        public string? NomeResponsavelFeedback { get; set; }
        public StatusFeedback Status { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string MensagemSolicitacao { get; set; }
    }
}
