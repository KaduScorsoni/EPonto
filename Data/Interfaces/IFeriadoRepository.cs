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
        Task<bool> CadastrarFeriado(FeriadoModel param);
        Task<List<FeriadoModel>> ListarFeriados();
        Task<bool> DeletarFeriado(int idFeriado);
    }
}
