using Domain.Entities;
using Domain.Entities.Comunicado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IComunicadoRepository
    {
        Task<int> CadastrarComunicado(ComunicadoModel param);
        Task<List<ComunicadoModel>> ListarComunicado();
        Task<int> DeletarComunicado(int idComunicado);
    }
}
