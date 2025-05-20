using Application.DTOs;
using Application.Interfaces;
using Data.Connections;
using Data.Interfaces;
using Data.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class JornadaTrabalhoService : IJornadaTrabalhoService
    {
        private readonly IJornadaTrabalhoRepository _jornadaTrabalhoRepository;
        private readonly DbSession _dbSession;

        public JornadaTrabalhoService(IJornadaTrabalhoRepository jornadaTrabalhoRepository, DbSession dbSession)
        {
            _jornadaTrabalhoRepository = jornadaTrabalhoRepository;
            _dbSession = dbSession;
        }
        public async Task<JornadaTrabalhoDTO> CriarJornadaTrabalhoAsync(JornadaTrabalhoModel jornadaTrabalho)
        {
            try
            {
                _dbSession.BeginTransaction();

                var jornadaTrabalhoExistente = await _jornadaTrabalhoRepository.ValidarJornadaExistente(jornadaTrabalho.NomeJornada);
                if (jornadaTrabalhoExistente != null)
                {
                    return new JornadaTrabalhoDTO
                    {
                        Sucesso = false,
                        Mensagem = "Já existe uma jornada de trabalho cadastrada com este nome."
                    };
                }
                var jornadaTrabalhoCriado = await _jornadaTrabalhoRepository.InserirAsync(jornadaTrabalho);

                _dbSession.Commit();
                return new JornadaTrabalhoDTO
                {
                    Sucesso = true,
                    Mensagem = "Jornada de trabalho cadastrada com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new JornadaTrabalhoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao criar jornada de trabalho: {ex.Message}"
                };
            }
        }

        public async Task<JornadaTrabalhoDTO> ObterJornadaTrabalhoPorIdAsync(int id)
        {
            try
            {
                var jornadaTrabalho = await _jornadaTrabalhoRepository.ObterPorIdAsync(id);
                if (jornadaTrabalho == null)
                {
                    return new JornadaTrabalhoDTO
                    {
                        Sucesso = false,
                        Mensagem = "Jornada de trabalho não encontrada."
                    };
                }

                return new JornadaTrabalhoDTO
                {
                    Sucesso = true,
                    Jornada = jornadaTrabalho
                };
            }
            catch (Exception ex)
            {
                return new JornadaTrabalhoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao buscar jornada de trabalho: {ex.Message}"
                };
            }
        }

        public async Task<JornadaTrabalhoDTO> ListarJornadasTrabalhoAsync()
        {
            try
            {
                var jornadaTrabalhos = await _jornadaTrabalhoRepository.ListarTodosAsync();
                return new JornadaTrabalhoDTO
                {
                    Sucesso = true,
                    Jornadas = jornadaTrabalhos
                };
            }
            catch (Exception ex)
            {
                return new JornadaTrabalhoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar jornadas de trabalhos: {ex.Message}"
                };
            }
        }
        public async Task<JornadaTrabalhoDTO> AtualizarJornadaTrabalhoAsync(JornadaTrabalhoModel jornadaTrabalho)
        {
            try
            {
                _dbSession.BeginTransaction();

                var sucesso = await _jornadaTrabalhoRepository.AtualizarAsync(jornadaTrabalho);
                _dbSession.Commit();

                return new JornadaTrabalhoDTO
                {
                    Sucesso = sucesso,
                    Mensagem = sucesso ? "Jornada de trabalho atualizado com sucesso." : "Falha ao atualizar jornada de trabalho."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new JornadaTrabalhoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao atualizar jornada de trabalho: {ex.Message}"
                };
            }
        }
        public async Task<JornadaTrabalhoDTO> ExcluirJornadaTrabalhoAsync(int id)
        {
            try
            {
                _dbSession.BeginTransaction();

                var sucesso = await _jornadaTrabalhoRepository.ExcluirAsync(id);
                _dbSession.Commit();

                return new JornadaTrabalhoDTO
                {
                    Sucesso = sucesso,
                    Mensagem = sucesso ? "Jornada de trabalho excluída com sucesso." : "Falha ao excluir jornada de trabalho."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new JornadaTrabalhoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao excluir jornada de trabalho: {ex.Message}"
                };
            }
        }
    }
}
