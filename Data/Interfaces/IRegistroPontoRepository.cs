using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRegistroPontoRepository
    {
        Task<int> InserirAsync(RegistroPontoModel ponto);
        Task<RegistroPontoModel> ObterPorIdAsync(int id);
        Task<IEnumerable<RegistroPontoModel>> ListarTodosAsync();
        Task<bool> AtualizarAsync(RegistroPontoModel ponto);
        Task<bool> ExcluirAsync(int id);
        Task<IEnumerable<RegistroPontoModel>> ObterRegistrosUsuarioAsync(int idUsuario);
        Task<bool> VerificarValidacaoMesAsync(int idUsuario, int anoReferencia, int mesReferencia);
        Task<int> ValidarMesAsync(int idUsuario, int anoReferencia, int mesReferencia, int statusValidacao);
    }
}
