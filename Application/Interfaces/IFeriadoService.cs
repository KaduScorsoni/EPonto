using Application.DTOs;
using Domain.Entities;
using Domain.Entities.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFeriadoService
    {
        Task<ResultadoDTO> CadastrarFeriado(FeriadoModel param);
        Task<FeriadoDTO> ListarFeriados();
        Task<ResultadoDTO> DeletarFeriado(int idFeriado);
        Task<ResultadoDTO> CadastrarFerias(FeriasModel param);
        Task<FeriasDTO> ListarFerias(int? idUsuario = null);
        Task<ResultadoDTO> DeletarFerias(int idFerias);
    }
}
