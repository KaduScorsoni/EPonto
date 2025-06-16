using Domain.Entities.Comunicado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ComunicadoDTO
    {
        public string Mensagem { get; set; }
        public bool Sucesso { get; set; }
        public List<ResultadoComunicadoModel> ListaComunicados { get; set; }

    }
}
