using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ICargoRepository
    {
        Task<int> InserirAsync(CargoModel cargo);
        Task<CargoModel> ObterPorIdAsync(int id);
        Task<IEnumerable<CargoModel>> ListarTodosAsync();
        Task<bool> AtualizarAsync(CargoModel cargo);
        Task<bool> ExcluirAsync(int id);
        Task<CargoModel> ValidarCargoExistente(string nomeCargo);

    }
}
