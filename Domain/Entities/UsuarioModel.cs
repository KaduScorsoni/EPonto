using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UsuarioModel
    {
        public int ID_USUARIO { get; set; }
        public string NOME { get; set; }
        public DateTime DATA_NASCIMENTO { get; set; }
        public string SENHA { get; set; }
        public int BATIDA_ATUAL { get; set; }
        public string EMAIL { get; set; }
        public int ID_CARGO { get; set; }
        public int ID_JORNADA { get; set; }
        public int TELEFONE { get; set; }
    }
}
