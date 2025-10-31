using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRelatorioRepository
    {
        Task<bool> RelatorioHorasExtras(DateOnly datInicio, DateOnly datFim, int IdCargo, long IdUsuario);
    }
}
