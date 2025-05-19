using Application.DTOs;
using Domain.Entities;
using Domain.Entities.Ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRegistroPontoService
    {
        Task<RegistroPontoDTO> CriarRegistroPontoAsync(RegistroPontoModel ponto);
        Task<RegistroPontoDTO> ObterRegistroPontoIdAsync(int id);
        Task<RegistroPontoDTO> ListarTodosRegistrosPontoAsync();
        Task<RegistroPontoDTO> ExcluirRegistroPontoAsync(int id);
        Task<RegistroPontoDTO> ObterRegistrosUsuarioAsync(int idUsuario);
        Task <RegistroPontoDTO> VerificarValidacaoMesAsync(int idUsuario);
        Task<RegistroPontoDTO> ValidarMesAsync(int idUsuario, int anoReferencia, int mesReferencia, int statusValidacao);
        Task<SolicitacaoAjusteDTO> ListarSolicitacoesAsync(int? status = null);
        Task<SolicitacaoAjusteDTO> CriarSolicitacaoAsync(SolicitacaoAjustePontoModel solicitacao);
        Task<SolicitacaoAjusteDTO> ObterSolicitacaoAltercaoPorIdAsync(int idSolicitacao);
        Task<SolicitacaoAjusteDTO> AprovarReprovarSolicitacaoAsync(int idSolicitacao, bool aprovado);
    }
}
