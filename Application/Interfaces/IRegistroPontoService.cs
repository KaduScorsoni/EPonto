using Application.DTOs;
using Domain.Entities;
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
        Task<RegistroPontoDTO> AtualizarRegistroPontoAsync(RegistroPontoModel ponto);
        Task<RegistroPontoDTO> ExcluirRegistroPontoAsync(int id);
        Task<RegistroPontoDTO> ObterRegistrosUsuarioAsync(int idUsuario);
        Task <RegistroPontoDTO> VerificarValidacaoMesAsync(int idUsuario);
        Task<RegistroPontoDTO> ValidarMesAsync(int idUsuario, int anoReferencia, int mesReferencia, int statusValidacao);

    }
}
