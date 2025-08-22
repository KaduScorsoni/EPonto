using Domain.Entities;
using Domain.Entities.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class FeedBackDTO
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public SolicitacaoFeedbackModel SolicitacaoFeedback { get; set; }
        public FeedbackModel Feedback { get; set; }
        public IEnumerable<SolicitacaoFeedbackModel> SolicitacoesFeedback { get; set; }
        public IEnumerable<FeedbackModel> Feedbacks { get; set; }
    }
}
