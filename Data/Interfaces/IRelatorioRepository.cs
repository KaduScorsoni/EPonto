using Domain.Entities.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRelatorioRepository
    {
        Task<List<RelHorasExtrasModel>> RelatorioHorasExtras(DateTime datInicio, DateTime datFim, int IdCargo, long IdUsuario);
    }
}
