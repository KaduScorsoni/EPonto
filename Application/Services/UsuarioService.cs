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
                        Mensagem = "Já existe um usuário com esse e-mail cadastrado."
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
            catch
            {
                _dbSession.Rollback();
                throw;
            }
        }


        public async Task<UsuarioModel> ObterUsuarioPorIdAsync(int id)
        {
            return await _usuarioRepository.ObterPorIdAsync(id);
        }

        public async Task<IEnumerable<UsuarioModel>> ListarTodosUsuariosAsync()
        {
            return await _usuarioRepository.ListarTodosAsync();
        }

        public async Task<bool> AtualizarUsuarioAsync(UsuarioModel usuario)
        {
            try
            {
                _dbSession.BeginTransaction();
                var sucesso = await _usuarioRepository.AtualizarAsync(usuario);
                _dbSession.Commit();
                return sucesso;
            }
            catch
            {
                _dbSession.Rollback();
                throw;
            }
        }

        public async Task<bool> ExcluirUsuarioAsync(int id)
        {
            try
            {
                _dbSession.BeginTransaction();
                var sucesso = await _usuarioRepository.ExcluirAsync(id);
                _dbSession.Commit();
                return sucesso;
            }
            catch
            {
                _dbSession.Rollback();
                throw;
            }
        }
    }
}
