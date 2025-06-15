using Application.DTOs;
using Application.Interfaces;
using Data.Connections;
using Data.Interfaces;
using Domain.Entities;
using Domain.Entities.Feriado_e_Ferias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FeriadoService : IFeriadoService
    {
        private readonly IFeriadoRepository _feriadoRepository;
        private readonly DbSession _dbSession;

        public FeriadoService(IFeriadoRepository feriadoRepository, DbSession dbSession)
        {
            _feriadoRepository = feriadoRepository;
            _dbSession = dbSession;
        }
        public async Task<ResultadoDTO> CadastrarFeriado(FeriadoModel param)
        {
            try
            {
                _dbSession.BeginTransaction();

                await _feriadoRepository.CadastrarFeriado(param);

                _dbSession.Commit();
                return new ResultadoDTO
                {
                    Sucesso = true,
                    Mensagem = "Feriado cadastrado com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new ResultadoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao cadastrar feriado: {ex.Message}"
                };
            }
        }

        public async Task<ResultadoDTO> DeletarFeriado(int idFeriado)
        {
            try
            {
                _dbSession.BeginTransaction();

                await _feriadoRepository.DeletarFeriado(idFeriado);

                _dbSession.Commit();

                return new ResultadoDTO
                {
                    Sucesso = true,
                    Mensagem = "Feriado excluido com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new ResultadoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao excluir feriado: {ex.Message}"
                };
            }
        }

        public async Task<FeriadoDTO> ListarFeriados()
        {
            try
            {
                List<ResultadoFeriadoModel> ListaFeriados = await _feriadoRepository.ListarFeriados();

                if (ListaFeriados.Count > 0)
                    return new FeriadoDTO
                    {
                        Sucesso = true,
                        Mensagem = "Feriado(s) listados com sucesso.",
                        ListaFeriados = ListaFeriados
                    };
                else
                    return new FeriadoDTO
                    {
                        Sucesso = true,
                        Mensagem = "Não há feriados cadastrados."
                    };
            }
            catch (Exception ex)
            {
                return new FeriadoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar feriado(s): {ex.Message}"
                };
            }
        }
        public async Task<ResultadoDTO> CadastrarFerias(FeriasModel param)
        {
            try
            {
                if (param.DatIncioFerias > param.DatFimFerias)
                    return new ResultadoDTO { Sucesso = false, Mensagem = "A data de Inicio não pode ser maior que a data Final!" };

                if (param.IdUsuario == 0)
                    param.IdUsuario = null;

                _dbSession.BeginTransaction();
                

                await _feriadoRepository.CadastrarFerias(param);

                _dbSession.Commit();
                return new ResultadoDTO
                {
                    Sucesso = true,
                    Mensagem = "Ferias cadastrada com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new ResultadoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao cadastrar ferias: {ex.Message}"
                };
            }
        }

        public async Task<FeriasDTO> ListarFerias(int? idUsuario = null)
        {
            try
            {
                List<ResultadoFeriasModel> ListaFerias = await _feriadoRepository.ListarFerias(idUsuario);

                if (ListaFerias.Count > 0)
                    return new FeriasDTO
                    {
                        Sucesso = true,
                        Mensagem = "Ferias listados com sucesso.",
                        ListaFerias = ListaFerias
                    };
                else
                    return new FeriasDTO
                    {
                        Sucesso = true,
                        Mensagem = "Não há ferias cadastradas."
                    };
            }
            catch (Exception ex)
            {
                return new FeriasDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar ferias(s): {ex.Message}"
                };
            }
        }

        public async Task<ResultadoDTO> DeletarFerias(int idFerias)
        {
            try
            {
                _dbSession.BeginTransaction();

                await _feriadoRepository.DeletarFerias(idFerias);

                _dbSession.Commit();

                return new ResultadoDTO
                {
                    Sucesso = true,
                    Mensagem = "Ferias excluida com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new ResultadoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao excluir ferias: {ex.Message}"
                };
            }
        }
    }
}
