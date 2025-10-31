using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RelatorioRepository : IRelatorioRepository
    {
        public Task<bool> RelatorioHorasExtras(DateOnly datInicio, DateOnly datFim, int IdCargo, long IdUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
