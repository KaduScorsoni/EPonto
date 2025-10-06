using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Perfil
{
    public class VinculoPerfilUsuario
    {
        public int IdUsuario { get; set; }
        public List<int> ListaIdPerfis { get; set; }
    }
}
