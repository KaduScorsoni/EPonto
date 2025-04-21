using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDTO> CriarUsuarioAsync(UsuarioModel usuario);
        Task<UsuarioDTO> ObterUsuarioPorIdAsync(int id);
        Task<UsuarioDTO> ListarTodosUsuariosAsync();
        Task<UsuarioDTO> AtualizarUsuarioAsync(UsuarioModel usuario);
        Task<UsuarioDTO> ExcluirUsuarioAsync(int id);
    }
}
