using Application.DTOs;
using Domain.Entities;
using Domain.Entities.Calendario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICalendarioService
    {
        Task<CalendarioDTO> MontaCalendario(int ano, int? idUsuario);
    }
}
