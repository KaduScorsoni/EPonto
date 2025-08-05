using Domain.Entities;
using Domain.Entities.Feriado_e_Ferias;
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
        Task<List<ResultadoFeriadoModel>> ListarFeriados();
        Task<int> DeletarFeriado(int idFeriado);
    }
}
