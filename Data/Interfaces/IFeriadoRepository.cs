using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IFeriadoRepository
    {
        Task<int> CadastrarFeriado(FeriadoModel param);
        Task<List<FeriadoModel>> ListarFeriados();
        Task<int> DeletarFeriado(int idFeriado);

        //Ferias
        Task<int> CadastrarFerias(FeriasModel param);
        Task<List<FeriasModel>> ListarFerias(int? idUsuario);
        Task<int> DeletarFerias(int idFerias);
    }
}
