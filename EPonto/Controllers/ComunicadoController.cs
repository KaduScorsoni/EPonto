using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComunicadoController : ControllerBase
    {
        private readonly IComunicadoService _comunicadoService;
        public ComunicadoController(IComunicadoService comunicadoService)
        {
            _comunicadoService = comunicadoService;
        }

        [HttpPost]
        [Route("CadastrarComunicado")]
        public async Task<ActionResult<CalendarioDTO>> BuscaCalendario(int ano, int? idUsuario = null)
        {
            try
            {
                CalendarioDTO auxResult = await _calendarioService.MontaCalendario(ano, idUsuario);

                if (auxResult.Sucesso)
                    return Ok(auxResult);

                return BadRequest(new CalendarioDTO { Sucesso = false, Mensagem = auxResult.Mensagem });
            }
            catch (Exception ex)
            {
                return BadRequest(new CalendarioDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
    }
}
