using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Data.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<int> InserirAsync(UsuarioModel usuario);
        Task<UsuarioModel> ObterPorIdAsync(int id);
        Task<IEnumerable<UsuarioModel>> ListarTodosAsync();
        Task<bool> AtualizarAsync(UsuarioModel usuario);
        Task<bool> ExcluirAsync(int id);
    }
}
