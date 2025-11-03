using Application.DTOs.Relatorios;
using Application.Interfaces;
using Data.Connections;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly IRelatorioRepository _relatorioRepository;
        private readonly IConfiguration _configuration;
        private readonly DbSession _dbSession;

        public RelatorioService(IRelatorioRepository relatorioRepository, IConfiguration configuration, DbSession dbSession)
        {
            _relatorioRepository = relatorioRepository;
            _configuration = configuration;
            _dbSession = dbSession;
        }

        public async Task<RelHorasExtrasDTO> RelatorioHorasExtras(DateTime datInicio, DateTime datFim, int IdCargo, long IdUsuario)
        {
            try
            {
                return new RelHorasExtrasDTO
                {
                    Sucesso = true,
                    Mensagem = "Relatorio gerado com sucesso.",
                    ListaItens = await _relatorioRepository.RelatorioHorasExtras(datInicio, datFim, IdCargo, IdUsuario)
                };
            }
            catch (Exception ex)
            {
                return new RelHorasExtrasDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao gerar relatório: {ex.Message}"
                };
            }
        }
    }
}
