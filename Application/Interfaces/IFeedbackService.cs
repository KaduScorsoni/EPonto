using Application.DTOs;
using Domain.Entities.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<FeedBackDTO> CriarSolicitacaoAsync(SolicitacaoFeedbackModel solicitacao);
        Task<FeedBackDTO> CriarFeedbackAsync(FeedbackModel feedback);
        Task<FeedBackDTO> ListarTodasSolicitacoesAsync();
        Task<FeedBackDTO> ListarTodosFeedbacksAsync();
        Task<FeedBackDTO> ObterSolicitacaoPorIdAsync(int id);
        Task<FeedBackDTO> ObterSolicitacoesPorUsuarioAsync(int idUsuario);
        Task<FeedBackDTO> ObterFeedbackPorIdAsync(int id);
        Task<FeedBackDTO> AtualizarSolicitacaoAsync(SolicitacaoFeedbackModel solicitacao);
        Task<FeedBackDTO> ExcluirSolicitacaoAsync(int id);
    }
}
