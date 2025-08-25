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
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly DbSession _dbSession;

        public FeedbackService(IFeedbackRepository feedbackRepository, DbSession dbSession)
        {
            _feedbackRepository = feedbackRepository;
            _dbSession = dbSession;
        }
        #region Solicitação Feedback
        public async Task<FeedBackDTO> CriarSolicitacaoAsync(SolicitacaoFeedbackModel solicitacao)
        {
            try
            {
                _dbSession.BeginTransaction();

                var solicitacaoFeedback = await _feedbackRepository.InserirSolicitacaoAsync(solicitacao);

                _dbSession.Commit();
                return new FeedBackDTO
                {
                    Sucesso = true,
                    Mensagem = "Solicitação de Feedback criada com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new FeedBackDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao criar solicitação de feedback: {ex.Message}"
                };
            }
        }
        public async Task<FeedBackDTO> ListarTodasSolicitacoesAsync()
        {
            try
            {
                var solicitacoes = await _feedbackRepository.ListarTodasSolicitacoesAsync();
                return new FeedBackDTO
                {
                    Sucesso = true,
                    SolicitacoesFeedback = solicitacoes
                };
            }
            catch (Exception ex)
            {
                return new FeedBackDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar usuários: {ex.Message}"
                };
            }
        }
        public async Task<FeedBackDTO> ObterSolicitacaoPorIdAsync(int id)
        {
            try
            {
                var solicitacao = await _feedbackRepository.ObterSolicitacaoPorIdAsync(id);
                if (solicitacao == null)
                {
                    return new FeedBackDTO
                    {
                        Sucesso = false,
                        Mensagem = "Solicitação de feedback não encontrada."
                    };
                }

                return new FeedBackDTO
                {
                    Sucesso = true,
                    SolicitacaoFeedback = solicitacao
                };
            }
            catch (Exception ex)
            {
                return new FeedBackDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao buscar solicitação de feedback: {ex.Message}"
                };
            }
        }

        public async Task<FeedBackDTO> AtualizarSolicitacaoAsync(SolicitacaoFeedbackModel solicitacao)
        {
            try
            {
                _dbSession.BeginTransaction();

                // Busca a solicitação existente
                var solicitacaoExistente = await _feedbackRepository.ObterSolicitacaoPorIdAsync(solicitacao.IdSolicitacaoFeedback, _dbSession.Transaction);

                if (solicitacaoExistente == null)
                {
                    _dbSession.Rollback();
                    return new FeedBackDTO
                    {
                        Sucesso = false,
                        Mensagem = "Solicitação de feedback não encontrada."
                    };
                }

                // Se já foi respondida, não pode atualizar
                if (solicitacaoExistente.Status != 0)
                {
                    _dbSession.Rollback();
                    return new FeedBackDTO
                    {
                        Sucesso = false,
                        Mensagem = "Não é possível atualizar uma solicitação já respondida."
                    };
                }

                // Atualiza a data para o momento da edição
                solicitacao.DataSolicitacao = DateTime.Now;

                // Chama o repositório para atualizar
                var sucesso = await _feedbackRepository.AtualizarSolicitacaoAsync(solicitacao, _dbSession.Transaction);

                if (sucesso)
                    _dbSession.Commit();
                else
                    _dbSession.Rollback();

                return new FeedBackDTO
                {
                    Sucesso = sucesso,
                    Mensagem = sucesso ? "Solicitação atualizada com sucesso." : "Falha ao atualizar solicitação."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new FeedBackDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao atualizar solicitação: {ex.Message}"
                };
            }
        }

        public async Task<FeedBackDTO> ExcluirSolicitacaoAsync(int id)
        {
            try
            {
                _dbSession.BeginTransaction();

                var solicitacao = await _feedbackRepository.ObterSolicitacaoPorIdAsync(id, _dbSession.Transaction);

                if (solicitacao == null)
                {
                    _dbSession.Rollback();
                    return new FeedBackDTO
                    {
                        Sucesso = false,
                        Mensagem = "Solicitação de feedback não encontrada."
                    };
                }

                if (solicitacao.Status != 0)
                {
                    _dbSession.Rollback();
                    return new FeedBackDTO
                    {
                        Sucesso = false,
                        Mensagem = "Não é possível excluir uma solicitação já respondida."
                    };
                }

                // Se status = 0, pode excluir usando a mesma transação
                var sucesso = await _feedbackRepository.ExcluirSolicitacaoAsync(id, _dbSession.Transaction);

                if (sucesso)
                    _dbSession.Commit();
                else
                    _dbSession.Rollback();

                return new FeedBackDTO
                {
                    Sucesso = sucesso,
                    Mensagem = sucesso ? "Solicitação excluída com sucesso." : "Falha ao excluir solicitação."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new FeedBackDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao excluir solicitação: {ex.Message}"
                };
            }
        }
        #endregion

        #region Feedback
        public async Task<FeedBackDTO> CriarFeedbackAsync(FeedbackModel feedback)
        {
            try
            {
                _dbSession.BeginTransaction();

                var Criarfeedback = await _feedbackRepository.InserirFeedbackAsync(feedback);

                _dbSession.Commit();
                return new FeedBackDTO
                {
                    Sucesso = true,
                    Mensagem = "Feedback criado com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new FeedBackDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao criar feedback: {ex.Message}"
                };
            }
        }

        public async Task<FeedBackDTO> ListarTodosFeedbacksAsync()
        {
            try
            {
                var feedbacks = await _feedbackRepository.ListarTodosFeedbacksAsync();
                return new FeedBackDTO
                {
                    Sucesso = true,
                    Feedbacks = feedbacks
                };
            }
            catch (Exception ex)
            {
                return new FeedBackDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar usuários: {ex.Message}"
                };
            }
        }
        public async Task<FeedBackDTO> ObterFeedbackPorIdAsync(int id)
        {
            try
            {
                var feedback = await _feedbackRepository.ObterFeedbackPorIdAsync(id);
                if (feedback == null)
                {
                    return new FeedBackDTO
                    {
                        Sucesso = false,
                        Mensagem = "Feedback não encontrada."
                    };
                }

                return new FeedBackDTO
                {
                    Sucesso = true,
                    Feedback = feedback
                };
            }
            catch (Exception ex)
            {
                return new FeedBackDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao buscar feedback: {ex.Message}"
                };
            }
        }
        #endregion
    }
}

