using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IJornadaTrabalhoRepository
    {
        Task<int> InserirAsync(JornadaTrabalhoModel jornada);
        Task<JornadaTrabalhoModel> ObterPorIdAsync(int id);
        Task<IEnumerable<JornadaTrabalhoModel>> ListarTodosAsync();
        Task<bool> AtualizarAsync(JornadaTrabalhoModel jornada);
        Task<bool> ExcluirAsync(int id);
        Task<JornadaTrabalhoModel> ValidarJornadaExistente(string nomeJornada);
        Task<TimeSpan> ObterJornadaDiariaUsuario(int idUsuario);
    }
}
