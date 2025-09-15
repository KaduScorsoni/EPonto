using Domain.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SolicitacaoAusenciaModel
    {
        public int? IdSolicitacaoAusencia { get; set; }
        public int IdUsuario { get; set; }
        public string? LinkArquivo { get; set; }
        public string MensagemSolicitacao { get; set; }
        public DateTime DataInicioAusencia { get; set; }
        public DateTime DataFimAusencia { get; set; }
        public DateTime? DataSolicitacao { get; set; }
        public StatusAlteracaoPonto? Status { get; set; }
    }
}
