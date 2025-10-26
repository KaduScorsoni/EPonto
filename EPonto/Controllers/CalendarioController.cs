using Application.DTOs;
using Application.Interfaces;
using Domain.Entities.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarioController : ControllerBase
    {
        private readonly ICalendarioService _calendarioService;
        public CalendarioController(ICalendarioService calendarioService)
        {
            _calendarioService = calendarioService;
        }

        /// <summary>
        /// Lista todos os eventos do calendário.
        /// </summary>
        /// <remarks>
        /// Retorna todos os eventos cadastrados no calendário. Requer autenticação.
        /// </remarks>
        /// <response code="200">Eventos retornados com sucesso</response>
        /// <response code="400">Erro ao listar eventos</response>
        [HttpGet]
        [Authorize]
        [Route("BuscaCalendario")]
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
