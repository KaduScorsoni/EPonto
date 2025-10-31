using Application.DTOs;
using Application.Interfaces;
using Data.Interfaces;
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

        public RelatorioService(IRelatorioRepository relatorioRepository, IConfiguration configuration)
        {
            _relatorioRepository = relatorioRepository;
            _configuration = configuration;
        }

        public Task<LoginDTO> RelatorioHorasExtras(DateOnly datInicio, DateOnly datFim, int IdCargo, long IdUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
