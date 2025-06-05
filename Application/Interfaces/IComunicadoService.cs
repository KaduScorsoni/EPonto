using Application.DTOs;
using Domain.Entities;
using Domain.Entities.Comunicado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IComunicadoService
    {
        Task<ResultadoDTO> CadastrarComunicado(ComunicadoModel param);
        Task<ComunicadoDTO> ListarComunicado();
        Task<ResultadoDTO> DeletarComunicado(int idComunicado);
    }
}
