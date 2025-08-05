using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFeriasService
    {
        Task<ResultadoDTO> CadastrarFerias(FeriasModel param);
        Task<FeriasDTO> ListarFerias(int? idUsuario = null);
        Task<ResultadoDTO> DeletarFerias(int idFerias);
        //Task<ResultadoDTO> CadastrarSolicitacaoFerias(SolicitacaoFeriasModel param);
    }
}
