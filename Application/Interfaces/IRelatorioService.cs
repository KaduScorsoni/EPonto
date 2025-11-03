using Application.DTOs.Relatorios;
using Domain.Entities.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRelatorioService
    {
        Task<RelHorasExtrasDTO> RelatorioHorasExtras(DateTime datInicio, DateTime datFim, int IdCargo, long IdUsuario);
    }
}
