using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioController : ControllerBase
    {
        private readonly IRelatorioService _relatorioService;
        public RelatorioController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        [HttpGet]
        [Route("RelatorioHorasExtras")]
        public async Task<ActionResult<LoginDTO>> RelatorioHorasExtras(DateOnly datInicio, DateOnly datFim, int IdCargo, long IdUsuario)
        {
            try
            {
                var auxResult = await _relatorioService.RelatorioHorasExtras(datInicio, datFim, IdCargo, IdUsuario);
                if (auxResult.Sucesso)
                    return Ok(auxResult);

                return BadRequest(auxResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new LoginDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
    }
}
