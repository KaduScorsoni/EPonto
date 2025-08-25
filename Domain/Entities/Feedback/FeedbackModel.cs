using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Feedback
{
    public class FeedbackModel
    {
        public int IdFeedback { get; set; }
        public int IdUsuarioFeedback { get; set; }
        public int IdAutorFeedback { get; set; }
        public int? IdSolicitacaoFeedback { get; set; }
        public string NomeUsuarioFeedback { get; set; }  // aqui
        public string NomeAutorFeedback { get; set; }     // aqui
        public DateTime DataRealizacao { get; set; }
        public string MensagemFeedback { get; set; }
        public FeedbackAvaliacao Avaliacao { get; set; }
    }
}