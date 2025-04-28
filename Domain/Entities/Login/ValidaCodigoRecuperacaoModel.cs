using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Login
{
    public class ValidaCodigoRecuperacaoModel
    {
        public string email { get; set; }
        public int codigo { get; set; }
    }
}
