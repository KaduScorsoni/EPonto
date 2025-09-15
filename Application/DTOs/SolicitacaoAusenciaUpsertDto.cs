using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SolicitacaoAusenciaUpsertDto
    {
        public int IdUsuario { get; set; }                
        public string? MensagemSolicitacao { get; set; } 
        public DateTime? DataInicioAusencia { get; set; }
        public DateTime? DataFimAusencia { get; set; }
        public IFormFile? Arquivo { get; set; }
    }

}
