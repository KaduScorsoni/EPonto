using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly DbSession _dbSession;

        public UsuarioService(IUsuarioRepository usuarioRepository, DbSession dbSession)
        {
            _usuarioRepository = usuarioRepository;
            _dbSession = dbSession;
        }

        public async Task CriarUsuarioAsync(UsuarioModel usuario)
        {
            try
            {
                _dbSession.BeginTransaction();

                var usuarioCriado = await _usuarioRepository.InserirAsync(usuario);
                _dbSession.Commit();

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
