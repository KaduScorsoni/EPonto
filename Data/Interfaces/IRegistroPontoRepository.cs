using Domain.Entities;
using Domain.Entities.Ponto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRegistroPontoRepository
    {
        Task<int> InserirAsync(RegistroPontoModel ponto);
        Task<RegistroPontoModel> ObterPorIdAsync(int id);
        Task<IEnumerable<RegistroPontoModel>> ListarTodosAsync();
        Task<bool> ExcluirAsync(int id);
        Task<IEnumerable<RegistroPontoModel>> ObterRegistrosUsuarioAsync(int idUsuario);
        Task<bool> VerificarValidacaoMesAsync(int idUsuario, int anoReferencia, int mesReferencia);
        Task<int> ValidarMesAsync(int idUsuario, int anoReferencia, int mesReferencia, int statusValidacao);
        Task<int> CriarSolicitacaoAsync(SolicitacaoAjustePontoModel solicitacao);
        Task<IEnumerable<SolicitacaoAjustePontoModel>> ListarSolicitacoesAsync();
        Task<SolicitacaoAjustePontoModel> ObterSolicitacaoAltercaoPorIdAsync(int idSolicitacao);
        Task<bool> AtualizarRegistroAsync(int idSolicitacao, bool aprovar, List<ItemAjustePontoModel> itensAlterados, DateTime dataRegistro, int idUsuario, IDbTransaction transaction);

    }
}
