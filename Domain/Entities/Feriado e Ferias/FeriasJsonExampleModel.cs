using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Feriado_e_Ferias
{
    public class FeriasRequestExampleModel : IExamplesProvider<FeriasModel>
    {
        public FeriasModel GetExamples()
        {
            return new FeriasModel
            {
                IdUsuario = 27,
                DscFerias = "Ferias Solicitadas pelo usuário",
                DatFimFerias = DateTime.Now,
                DatIncioFerias = DateTime.Now
            };
        }
    }
}
