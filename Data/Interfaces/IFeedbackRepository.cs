using Domain.Entities;
using Domain.Entities.Feedback;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<int> InserirSolicitacaoAsync(SolicitacaoFeedbackModel solicitacao);
        Task<int> InserirFeedbackAsync(FeedbackModel feedback);
        Task<IEnumerable<SolicitacaoFeedbackModel>> ListarTodasSolicitacoesAsync();
        Task<IEnumerable<FeedbackModel>> ListarTodosFeedbacksAsync();
        Task<SolicitacaoFeedbackModel> ObterSolicitacaoPorIdAsync(int id, IDbTransaction? transaction = null);
        Task<IEnumerable<SolicitacaoFeedbackModel>> ObterSolicitacoesPorUsuarioAsync(int idUsuario, IDbTransaction? transaction = null);
        Task<IEnumerable<SolicitacaoFeedbackModel>> ObterSolicitacoesResponsavelAsync(int idResponsavel, IDbTransaction? transaction = null);
        Task<FeedbackModel> ObterFeedbackPorIdAsync(int id);
        Task<IEnumerable<FeedbackModel>> ObterFeedbacksPorUsuarioAsync(int idUsuario);
        Task<bool> AtualizarSolicitacaoAsync(SolicitacaoFeedbackModel solicitacao,IDbTransaction? transaction = null);
        Task<bool> ExcluirSolicitacaoAsync(int id, IDbTransaction? transaction = null);
    }
}
