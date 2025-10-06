using Application.DTOs;
using Domain.Entities;
using Domain.Entities.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPerfilService
    {
        Task<ResultadoDTO> CadastrarPerfil(PerfilModel param);
        Task<ResultadoDTO> CadastrarVinculoPerfilUsuario(VinculoPerfilUsuario param);
        Task<PerfilDTO> ListarPerfis();
        Task<PerfilDTO> ListarPerfil(int idPerfil);
        Task<ResultadoDTO> EditarPerfil(PerfilModel param);
        Task<ResultadoDTO> RemoverPerfil(int idPerfil);
    }
}
