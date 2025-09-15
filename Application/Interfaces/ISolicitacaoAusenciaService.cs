using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISolicitacaoAusenciaService
    {
        Task<SolicitacaoAusenciaDTO> ListarTodosSolicitacaoAusenciaAsync();
        Task<SolicitacaoAusenciaDTO> ObterSolicitacaoAusenciaPorIdAsync(int id);
        Task<SolicitacaoAusenciaDTO> ObterSolicitacoesAusenciaPorUsuarioAsync(int idUsuario);
        Task<SolicitacaoAusenciaDTO> CriarSolicitacaoAusenciaAsync(SolicitacaoAusenciaModel solicitacao);
        Task<SolicitacaoAusenciaDTO> AtualizarSolicitacaoAsync(SolicitacaoAusenciaModel solicitacao);
        Task<SolicitacaoAusenciaDTO> ExcluirSolicitacaoAsync(int id);
        Task<SolicitacaoAusenciaDTO> ResponderSolicitacaoAsync(int idSolicitacao, bool aprovar);
    }
}
