using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICargoService
    {
        Task<CargoDTO> CriarCargoAsync(CargoModel cargo);
        Task<CargoDTO> ObterCargoPorIdAsync(int id);
        Task<CargoDTO> ListarTodosCargosAsync();
        Task<CargoDTO> AtualizarCargoAsync(CargoModel cargo);
        Task<CargoDTO> ExcluirCargoAsync(int id);
    }
}
