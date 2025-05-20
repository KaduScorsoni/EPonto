using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IJornadaTrabalhoService
    {
        Task<JornadaTrabalhoDTO> CriarJornadaTrabalhoAsync(JornadaTrabalhoModel jornada);
        Task<JornadaTrabalhoDTO> ObterJornadaTrabalhoPorIdAsync(int id);
        Task<JornadaTrabalhoDTO> ListarJornadasTrabalhoAsync();
        Task<JornadaTrabalhoDTO> AtualizarJornadaTrabalhoAsync(JornadaTrabalhoModel jornada);
        Task<JornadaTrabalhoDTO> ExcluirJornadaTrabalhoAsync(int id);
    }
}
