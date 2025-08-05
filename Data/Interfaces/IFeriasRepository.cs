using Domain.Entities.Feriado_e_Ferias;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IFeriasRepository
    {
        Task<int> CadastrarFerias(FeriasModel param);
        Task<List<ResultadoFeriasModel>> ListarFerias(int? idUsuario);
        Task<int> DeletarFerias(int idFerias);

    }
}
