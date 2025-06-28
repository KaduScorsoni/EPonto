using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BancoHorasController : ControllerBase
    {
        private readonly IBancoHorasService _bancoHorasService;

        public BancoHorasController(IBancoHorasService bancoHorasService)
        {
            _bancoHorasService = bancoHorasService;
        }

        [HttpPost]
        [Route("Processar/{idUsuario}")]
        public async Task<IActionResult> ProcessarBancoHoras(int idUsuario, [FromQuery] DateTime data)
        {
            var resultado = await _bancoHorasService.ProcessarBancoHorasDiarioAsync(idUsuario, data);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpGet]
        [Route("SaldosDiarios/{idUsuario}")]
        public async Task<IActionResult> ObterSaldosDiarios(int idUsuario)
        {
            var resultado = await _bancoHorasService.ObterSaldosUsuarioAsync(idUsuario);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpGet]
        [Route("Atual/{idUsuario}")]
        public async Task<IActionResult> ObterBancoHorasAtual(int idUsuario)
        {
            var resultado = await _bancoHorasService.ObterBancoHorasAtualAsync(idUsuario);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpGet]
        [Route("HorasTrabalhadasMes/{idUsuario}")]
        public async Task<IActionResult> ObterHorasTrabalhadasPorMes(int idUsuario)
        {
            var resultado = await _bancoHorasService.ObterHorasTrabalhadasPorMesAsync(idUsuario);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpGet]
        [Route("HorasExtrasMes/{idUsuario}")]
        public async Task<IActionResult> ObterHorasExtrasPorMes(int idUsuario)
        {
            var resultado = await _bancoHorasService.ObterHorasExtrasPorMesAsync(idUsuario);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
    }
}
