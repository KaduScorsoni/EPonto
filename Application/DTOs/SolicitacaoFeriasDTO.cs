using Domain.Entities.Feriado_e_Ferias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SolicitacaoFeriasDTO
    {
        public string Mensagem { get; set; }
        public bool Sucesso { get; set; }
        public List<ResultadoSolicitacaoFeriasModel> ListarSolicitacoesFerias { get; set; } 
    }
}
