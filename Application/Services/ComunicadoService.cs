using Application.DTOs;
using Application.Interfaces;
using Data.Connections;
using Data.Interfaces;
using Data.Repositories;
using Domain.Entities;
using Domain.Entities.Comunicado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ComunicadoService : IComunicadoService
    {
        private readonly IComunicadoRepository _comunicadoRepository;
        private readonly DbSession _dbSession;

        public ComunicadoService(IComunicadoRepository comunicadoRepository, DbSession dbSession)
        {
            _comunicadoRepository = comunicadoRepository;
            _dbSession = dbSession;
        }
        public async Task<ResultadoDTO> CadastrarComunicado(ComunicadoModel param)
        {
            try
            {
                _dbSession.BeginTransaction();

                await _comunicadoRepository.CadastrarComunicado(param);

                _dbSession.Commit();
                return new ResultadoDTO
                {
                    Sucesso = true,
                    Mensagem = "Comunicado cadastrado com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new ResultadoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao cadastrar comunicado: {ex.Message}"
                };
            }
        }

        public async Task<ResultadoDTO> DeletarComunicado(int idComunicado)
        {
            try
            {
                _dbSession.BeginTransaction();

                await _comunicadoRepository.DeletarComunicado(idComunicado);

                _dbSession.Commit();

                return new ResultadoDTO
                {
                    Sucesso = true,
                    Mensagem = "Comunicado excluído com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new ResultadoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao excluir comunicado: {ex.Message}"
                };
            }
        }

        public async Task<ComunicadoDTO> ListarComunicado()
        {
            try
            {
                List<ResultadoComunicadoModel> ListaComunicados = await _comunicadoRepository.ListarComunicado();

                if (ListaComunicados.Count > 0)
                    return new ComunicadoDTO
                    {
                        Sucesso = true,
                        Mensagem = "Comunicado(s) listados com sucesso.",
                        ListaComunicados = ListaComunicados
                    };
                else
                    return new ComunicadoDTO
                    {
                        Sucesso = true,
                        Mensagem = "Não há comunicados cadastrados."
                    };
            }
            catch (Exception ex)
            {
                return new ComunicadoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar feriado(s): {ex.Message}"
                };
            }
        }
    }
}
