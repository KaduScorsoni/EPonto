using Application.DTOs;
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
    public class FeriasService
    {
        private readonly IFeriasRepository _FeriasRepository;
        private readonly DbSession _dbSession;

        public FeriasService(IFeriasRepository FeriasRepository, DbSession dbSession)
        {
            _FeriasRepository = FeriasRepository;
            _dbSession = dbSession;
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


                await _FeriasRepository.CadastrarFerias(param);

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
                List<ResultadoFeriasModel> ListaFerias = await _FeriasRepository.ListarFerias(idUsuario);

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

                await _FeriasRepository.DeletarFerias(idFerias);

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
