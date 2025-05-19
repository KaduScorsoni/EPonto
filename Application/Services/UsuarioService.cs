using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Data.Connections;
using Data.Interfaces;
using Data.Repositories;
using Domain.Entities;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILoginService _loginService;
        private readonly DbSession _dbSession;

        public UsuarioService(IUsuarioRepository usuarioRepository, DbSession dbSession, ILoginService loginService)
        {
            _usuarioRepository = usuarioRepository;
            _dbSession = dbSession;
            _loginService = loginService;
        }

        public async Task<UsuarioDTO> CriarUsuarioAsync(UsuarioModel usuario)
        {
            try
            {
                _dbSession.BeginTransaction();

                var usuarioExistente = await _usuarioRepository.ValidarEmail(usuario.Email);
                if (usuarioExistente != null)
                {
                    return new UsuarioDTO
                    {
                        Sucesso = false,
                        Mensagem = "Já existe um usuário com este e-mail cadastrado."
                    };
                }
                usuario.Senha = _loginService.HashPassword(usuario.Senha);
                var usuarioCriado = await _usuarioRepository.InserirAsync(usuario);

                _dbSession.Commit();
                return new UsuarioDTO
                {
                    Sucesso = true,
                    Mensagem = "Usuário cadastrado com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new UsuarioDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao criar usuário: {ex.Message}"
                };
            }
        }


        public async Task<UsuarioDTO> ObterUsuarioPorIdAsync(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterPorIdAsync(id);
                if (usuario == null)
                {
                    return new UsuarioDTO
                    {
                        Sucesso = false,
                        Mensagem = "Usuário não encontrado."
                    };
                }

                return new UsuarioDTO
                {
                    Sucesso = true,
                    Usuario = usuario
                };
            }
            catch (Exception ex)
            {
                return new UsuarioDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao buscar usuário: {ex.Message}"
                };
            }
        }

        public async Task<UsuarioDTO> ListarTodosUsuariosAsync()
        {
            try
            {
                var usuarios = await _usuarioRepository.ListarTodosAsync();
                return new UsuarioDTO
                {
                    Sucesso = true,
                    Usuarios = usuarios
                };
            }
            catch (Exception ex)
            {
                return new UsuarioDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar usuários: {ex.Message}"
                };
            }
        }

        public async Task<UsuarioDTO> AtualizarUsuarioAsync(UsuarioModel usuario)
        {
            try
            {
                _dbSession.BeginTransaction();

                usuario.Senha = _loginService.HashPassword(usuario.Senha);

                var sucesso = await _usuarioRepository.AtualizarAsync(usuario);
                _dbSession.Commit();

                return new UsuarioDTO
                {
                    Sucesso = sucesso,
                    Mensagem = sucesso ? "Usuário atualizado com sucesso." : "Falha ao atualizar usuário."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new UsuarioDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao atualizar usuário: {ex.Message}"
                };
            }
        }
        public async Task<UsuarioDTO> ExcluirUsuarioAsync(int id)
        {
            try
            {
                _dbSession.BeginTransaction();

                var sucesso = await _usuarioRepository.ExcluirAsync(id);
                _dbSession.Commit();

                return new UsuarioDTO
                {
                    Sucesso = sucesso,
                    Mensagem = sucesso ? "Usuário excluído com sucesso." : "Falha ao excluir usuário."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new UsuarioDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao excluir usuário: {ex.Message}"
                };
            }
        }
    }
}
