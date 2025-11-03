using Domain.Entities.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Relatorios
{
    public class RelHorasExtrasDTO
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public List<RelHorasExtrasModel> ListaItens { get; set; }
    }
}
