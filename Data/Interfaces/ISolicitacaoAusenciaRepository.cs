using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ISolicitacaoAusenciaRepository
    {
        Task<IEnumerable<SolicitacaoAusenciaModel>> ListarTodosAsync();
        Task<SolicitacaoAusenciaModel> ObterPorIdAsync(int id, IDbTransaction? transaction = null);
        Task<IEnumerable<SolicitacaoAusenciaModel>> ObterSolicitacoesPorUsuarioAsync(int idUsuario, IDbTransaction? transaction = null);
        Task<int> InserirAsync(SolicitacaoAusenciaModel solicitacao);
        Task<bool> AtualizarSolicitacaoAsync(SolicitacaoAusenciaModel solicitacao, IDbTransaction? transaction = null);
        Task<bool> ExcluirSolicitacaoAsync(int id, IDbTransaction? transaction = null);
        Task<bool> AtualizarStatusAsync(int idSolicitacao, int status, IDbTransaction? transaction = null);
    }
}
