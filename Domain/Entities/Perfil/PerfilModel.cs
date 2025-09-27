using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Perfil
{
    public class PerfilModel
    {
        public int IdPerfil { get; set; }
        public string DscPerfil { get; set; }
        public int IndAcessoAdmin { get; set; }
        public int IndPermiteCadastrar { get; set; }
        public int IndPermiteEditar { get; set; }
        public int IndPermiteDeletar { get; set; }
        public int IndPermiteRegularSolicitacoes { get; set; }
    }
}
