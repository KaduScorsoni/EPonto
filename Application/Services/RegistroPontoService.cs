using Application.DTOs;
using Application.Interfaces;
using Data.Connections;
using Data.Interfaces;
using Data.Repositories;
using Domain.Entities;
using Domain.Entities.Ponto;
using Domain.Enum;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RegistroPontoService : IRegistroPontoService
    {
        private readonly IRegistroPontoRepository _registroPontoRepository;
        private readonly DbSession _dbSession;

        public RegistroPontoService(IRegistroPontoRepository registroPontoRepository, DbSession dbSession)
        {
            _registroPontoRepository = registroPontoRepository;
            _dbSession = dbSession;
        }

        public async Task<RegistroPontoDTO> CriarRegistroPontoAsync(RegistroPontoModel ponto)
        {
            try
            {
                _dbSession.BeginTransaction();
                var usuarioCriado = await _registroPontoRepository.InserirAsync(ponto);
                _dbSession.Commit();
                return new RegistroPontoDTO
                {
                    Sucesso = true,
                    Mensagem = "Registro cadastrado com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new RegistroPontoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao criar registro: {ex.Message}"
                };
            }
        }

        public async Task<RegistroPontoDTO> ObterRegistroPontoIdAsync(int id)
        {
            try
            {
                var ponto = await _registroPontoRepository.ObterPorIdAsync(id);
                if (ponto == null)
                {
                    return new RegistroPontoDTO
                    {
                        Sucesso = false,
                        Mensagem = "Registro não encontrado."
                    };
                }

                return new RegistroPontoDTO
                {
                    Sucesso = true,
                    RegistroPonto = ponto
                };
            }
            catch (Exception ex)
            {
                return new RegistroPontoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao buscar registro: {ex.Message}"
                };
            }
        }

        public async Task<RegistroPontoDTO> ListarTodosRegistrosPontoAsync()
        {
            try
            {
                var registros = await _registroPontoRepository.ListarTodosAsync();
                return new RegistroPontoDTO
                {
                    Sucesso = true,
                    Registros = registros
                };
            }
            catch (Exception ex)
            {
                return new RegistroPontoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar registros: {ex.Message}"
                };
            }
        }

        public async Task<RegistroPontoDTO> ExcluirRegistroPontoAsync(int id)
        {
            try
            {
                _dbSession.BeginTransaction();

                var sucesso = await _registroPontoRepository.ExcluirAsync(id);
                _dbSession.Commit();

                return new RegistroPontoDTO
                {
                    Sucesso = sucesso,
                    Mensagem = sucesso ? "Registro excluído com sucesso." : "Falha ao excluir registro."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new RegistroPontoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao excluir registro: {ex.Message}"
                };
            }
        }

        public async Task<RegistroPontoDTO> ObterRegistrosUsuarioAsync(int idUsuario)
        {
            try
            {
                var registros = await _registroPontoRepository.ObterRegistrosUsuarioAsync(idUsuario);
                return new RegistroPontoDTO
                {
                    Sucesso = true,
                    Registros = registros
                };
            }
            catch (Exception ex)
            {
                return new RegistroPontoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar registros: {ex.Message}"
                };
            }
        }

        public async Task<RegistroPontoDTO> VerificarValidacaoMesAsync(int idUsuario)
        {
            try
            {
                // Lógica de validação do mês anterior
                int mesReferencia = DateTime.Now.Month - 1;
                int anoReferencia = DateTime.Now.Year;

                if (mesReferencia == 0) // Se for janeiro, ajusta para dezembro do ano anterior
                {
                    mesReferencia = 12;
                    anoReferencia = DateTime.Now.Year - 1;
                }

                var validacao = await _registroPontoRepository.VerificarValidacaoMesAsync(idUsuario, anoReferencia, mesReferencia);
                if (validacao == false)
                {
                    return new RegistroPontoDTO
                    {
                        Sucesso = true,
                        Mensagem = "Existem validações mensais pendentes no sistema"
                    };
                }
                return new RegistroPontoDTO
                {
                    Sucesso = true
                };
            }
            catch (Exception ex)
            {
                return new RegistroPontoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao verificar validação mensal: {ex.Message}"
                };
            }
        }

        public async Task<RegistroPontoDTO> ValidarMesAsync(int idUsuario, int anoReferencia, int mesReferencia, int statusValidacao)
        {
            try
            {
                _dbSession.BeginTransaction();
                var validar = await _registroPontoRepository.ValidarMesAsync(idUsuario, anoReferencia, mesReferencia, statusValidacao);
                _dbSession.Commit();
                return new RegistroPontoDTO
                {
                    Sucesso = true,
                    Mensagem = "Mes validado com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new RegistroPontoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao validar mes: {ex.Message}"
                };
            }
        }

      

        public async Task<SolicitacaoAjusteDTO> CriarSolicitacaoAsync(SolicitacaoAjustePontoModel solicitacao)
        {
            try
            {
                _dbSession.BeginTransaction();
                var idSolicitacao = await _registroPontoRepository.CriarSolicitacaoAsync(solicitacao);
                _dbSession.Commit();

                return new SolicitacaoAjusteDTO
                {
                    Sucesso = true,
                    Mensagem = "Solicitação criada com sucesso.",
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new SolicitacaoAjusteDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao criar solicitação: {ex.Message}"
                };
            }
        }

        public async Task<SolicitacaoAjusteDTO> ListarSolicitacoesAsync(int? status = null)
        {
            try
            {
                var solicitacoes = await _registroPontoRepository.ListarSolicitacoesAsync(status);

                return new SolicitacaoAjusteDTO
                {
                    Sucesso = true,
                    Solicitacoes = solicitacoes
                };
            }
            catch (Exception ex)
            {
                return new SolicitacaoAjusteDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar solicitações: {ex.Message}"
                };
            }
        }

        public async Task<SolicitacaoAjusteDTO> ObterSolicitacaoAltercaoPorIdAsync(int idSolicitacao)
        {
            try
            {
                var solicitacao = await _registroPontoRepository.ObterSolicitacaoAltercaoPorIdAsync(idSolicitacao);

                if (solicitacao == null)
                {
                    return new SolicitacaoAjusteDTO
                    {
                        Sucesso = false,
                        Mensagem = "Solicitação não encontrada."
                    };
                }

                return new SolicitacaoAjusteDTO
                {
                    Sucesso = true,
                    Solicitacoes = new List<SolicitacaoAjustePontoModel> { solicitacao }
                };
            }
            catch (Exception ex)
            {
                return new SolicitacaoAjusteDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar solicitação: {ex.Message}"
                };
            }
        }

        public async Task<SolicitacaoAjusteDTO> AprovarReprovarSolicitacaoAsync(int idSolicitacao, bool aprovado)
        {
            try
            {
                _dbSession.BeginTransaction();

                // Obter a solicitação de ajuste por ID
                var solicitacao = await _registroPontoRepository.ObterSolicitacaoAltercaoPorIdAsync(idSolicitacao);
                if (solicitacao == null)
                {
                    return new SolicitacaoAjusteDTO
                    {
                        Sucesso = false,
                        Mensagem = "Solicitação não encontrada."
                    };
                }

                // Se for aprovado, atualiza os registros de ponto
                if (aprovado)
                {
                    // Preparar a lista de itens a serem atualizados
                    var itensAlterados = solicitacao.Itens.Select(item => new ItemAjustePontoModel
                    {
                        HoraRegistro = item.HoraRegistro,
                        IdTipoRegistroPonto = item.IdTipoRegistroPonto
                    }).ToList();

                    bool sucessoAtualizacao = await _registroPontoRepository.AtualizarRegistroAsync(
                        idSolicitacao,
                        aprovado,
                        itensAlterados,
                        solicitacao.DataRegistroAlteracao,
                        solicitacao.IdSolicitante,
                        _dbSession.Transaction
                    );

                    if (!sucessoAtualizacao)
                    {
                        _dbSession.Rollback();
                        return new SolicitacaoAjusteDTO
                        {
                            Sucesso = false,
                            Mensagem = "Erro ao aprovar a solicitação."
                        };
                    }

                    solicitacao.StatusSolicitacao = StatusAlteracaoPonto.Aprovada;
                    solicitacao.DataResposta = DateTime.Now;
                }
                else
                {
                    solicitacao.StatusSolicitacao = StatusAlteracaoPonto.Reprovada;
                    solicitacao.DataResposta = DateTime.Now;
                    bool sucessoAtualizacao = await _registroPontoRepository.AtualizarRegistroAsync(
                        idSolicitacao,
                        aprovado,
                        new List<ItemAjustePontoModel>(),
                        solicitacao.DataRegistroAlteracao,
                        solicitacao.IdSolicitante,
                        _dbSession.Transaction
                    );

                    if (!sucessoAtualizacao)
                    {
                        _dbSession.Rollback();
                        return new SolicitacaoAjusteDTO
                        {
                            Sucesso = false,
                            Mensagem = "Erro ao reprovar a solicitação."
                        };
                    }
                }

                _dbSession.Commit();

                return new SolicitacaoAjusteDTO
                {
                    Sucesso = true,
                    Mensagem = aprovado ? "Solicitação aprovada com sucesso." : "Solicitação reprovada com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new SolicitacaoAjusteDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao processar solicitação: {ex.Message}"
                };
            }
        }

    }
}
