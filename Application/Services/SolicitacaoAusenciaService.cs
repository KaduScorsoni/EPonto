using Application.DTOs;
using Application.Interfaces;
using Data.Connections;
using Data.Interfaces;
using Data.Repositories;
using Domain.Entities;
using Domain.Entities.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SolicitacaoAusenciaService : ISolicitacaoAusenciaService
    {
        private readonly ISolicitacaoAusenciaRepository _solicitacoAusenciaRepository;
        private readonly DbSession _dbSession;

        public SolicitacaoAusenciaService(ISolicitacaoAusenciaRepository solicitacoAusenciaRepository, DbSession dbSession)
        {
            _solicitacoAusenciaRepository = solicitacoAusenciaRepository;
            _dbSession = dbSession;
        }
        public async Task<SolicitacaoAusenciaDTO> ObterSolicitacaoAusenciaPorIdAsync(int id)
        {
            try
            {
                var solicitacao = await _solicitacoAusenciaRepository.ObterPorIdAsync(id);
                if (solicitacao == null)
                {
                    return new SolicitacaoAusenciaDTO
                    {
                        Sucesso = false,
                        Mensagem = "Solicitacao não encontrado."
                    };
                }

                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = true,
                    SolicitacaoAusencia = solicitacao
                };
            }
            catch (Exception ex)
            {
                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao buscar solicitacao: {ex.Message}"
                };
            }
        }

        public async Task<SolicitacaoAusenciaDTO> ListarTodosSolicitacaoAusenciaAsync()
        {
            try
            {
                var solicitacoes = await _solicitacoAusenciaRepository.ListarTodosAsync();
                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = true,
                    Solicitacoes = solicitacoes
                };
            }
            catch (Exception ex)
            {
                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar solicitacoes: {ex.Message}"
                };
            }
        }
        public async Task<SolicitacaoAusenciaDTO> ObterSolicitacoesAusenciaPorUsuarioAsync(int idUsuario)
        {
            try
            {
                var solicitacoes = await _solicitacoAusenciaRepository.ObterSolicitacoesPorUsuarioAsync(idUsuario);

                if (solicitacoes == null || !solicitacoes.Any())
                {
                    return new SolicitacaoAusenciaDTO
                    {
                        Sucesso = false,
                        Mensagem = "Nenhuma solicitação de ausencia encontrada para este usuário."
                    };
                }

                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = true,
                    Solicitacoes = solicitacoes.ToList()
                };
            }
            catch (Exception ex)
            {
                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao buscar solicitações de ausencia: {ex.Message}"
                };
            }
        }
        public async Task<SolicitacaoAusenciaDTO> CriarSolicitacaoAusenciaAsync(SolicitacaoAusenciaModel solicitacao)
        {
            try
            {
                _dbSession.BeginTransaction();

                var solicitacaoAusencia = await _solicitacoAusenciaRepository.InserirAsync(solicitacao);

                _dbSession.Commit();
                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = true,
                    Mensagem = "Solicitação cadastrada com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao criar Solicitação: {ex.Message}"
                };
            }
        }

        public async Task<SolicitacaoAusenciaDTO> AtualizarSolicitacaoAsync(SolicitacaoAusenciaModel solicitacao)
        {
            // 1) Garante que o ID foi enviado
            if (solicitacao?.IdSolicitacaoAusencia is null)
            {
                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = false,
                    Mensagem = "Id da solicitação é obrigatório."
                };
            }

            try
            {
                _dbSession.BeginTransaction();

                var id = solicitacao.IdSolicitacaoAusencia.Value;

                var solicitacaoExistente = await _solicitacoAusenciaRepository
                    .ObterPorIdAsync(id, _dbSession.Transaction);

                if (solicitacaoExistente == null)
                {
                    _dbSession.Rollback();
                    return new SolicitacaoAusenciaDTO
                    {
                        Sucesso = false,
                        Mensagem = "Solicitação de ausência não encontrada."
                    };
                }

                var status = solicitacaoExistente.Status ?? 0;
                if (status != 0)
                {
                    _dbSession.Rollback();
                    return new SolicitacaoAusenciaDTO
                    {
                        Sucesso = false,
                        Mensagem = "Não é possível atualizar uma solicitação já respondida."
                    };
                }

                solicitacao.DataSolicitacao = DateTime.Now;

                var sucesso = await _solicitacoAusenciaRepository
                    .AtualizarSolicitacaoAsync(solicitacao, _dbSession.Transaction);

                if (sucesso) _dbSession.Commit();
                else _dbSession.Rollback();

                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = sucesso,
                    Mensagem = sucesso ? "Solicitação atualizada com sucesso." : "Falha ao atualizar solicitação."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao atualizar solicitação: {ex.Message}"
                };
            }
        }

        public async Task<SolicitacaoAusenciaDTO> ExcluirSolicitacaoAsync(int id)
        {
            try
            {
                _dbSession.BeginTransaction();

                var solicitacao = await _solicitacoAusenciaRepository.ObterPorIdAsync(id, _dbSession.Transaction);

                if (solicitacao == null)
                {
                    _dbSession.Rollback();
                    return new SolicitacaoAusenciaDTO
                    {
                        Sucesso = false,
                        Mensagem = "Solicitação de feedback não encontrada."
                    };
                }

                if (solicitacao.Status != 0)
                {
                    _dbSession.Rollback();
                    return new SolicitacaoAusenciaDTO
                    {
                        Sucesso = false,
                        Mensagem = "Não é possível excluir uma solicitação já respondida."
                    };
                }

                // Se status = 0, pode excluir usando a mesma transação
                var sucesso = await _solicitacoAusenciaRepository.ExcluirSolicitacaoAsync(id, _dbSession.Transaction);

                if (sucesso)
                    _dbSession.Commit();
                else
                    _dbSession.Rollback();

                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = sucesso,
                    Mensagem = sucesso ? "Solicitação excluída com sucesso." : "Falha ao excluir solicitação."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao excluir solicitação: {ex.Message}"
                };
            }
        }
        public async Task<SolicitacaoAusenciaDTO> ResponderSolicitacaoAsync(int idSolicitacao, bool aprovar)
        {
            try
            {
                _dbSession.BeginTransaction();

                var solicitacao = await _solicitacoAusenciaRepository.ObterPorIdAsync(idSolicitacao, _dbSession.Transaction);

                if (solicitacao == null)
                {
                    _dbSession.Rollback();
                    return new SolicitacaoAusenciaDTO { Sucesso = false, Mensagem = "Solicitação não encontrada." };
                }

                if (solicitacao.Status != 0)
                {
                    _dbSession.Rollback();
                    return new SolicitacaoAusenciaDTO { Sucesso = false, Mensagem = "Solicitação já foi respondida." };
                }

                int status = aprovar ? 1 : 2; // 1 = aprovado, 2 = reprovado

                bool atualizado = await _solicitacoAusenciaRepository.AtualizarStatusAsync(idSolicitacao, status, _dbSession.Transaction);

                if (aprovar)
                {
                    // Se aprovado → inativar dias no banco de horas
                    var bancoHorasRepo = new BancoHorasRepository(_dbSession, null);
                    await bancoHorasRepo.InativarSaldosPorPeriodoAsync(solicitacao.IdUsuario,
                                                                       solicitacao.DataInicioAusencia,
                                                                       solicitacao.DataFimAusencia,
                                                                       _dbSession.Transaction);
                }

                _dbSession.Commit();
                return new SolicitacaoAusenciaDTO
                {
                    Sucesso = true,
                    Mensagem = aprovar ? "Solicitação aprovada e dias inativados no banco de horas." : "Solicitação reprovada."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new SolicitacaoAusenciaDTO { Sucesso = false, Mensagem = $"Erro ao responder solicitação: {ex.Message}" };
            }
        }

    }
}
