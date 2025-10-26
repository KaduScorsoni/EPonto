using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Processa o banco de horas diário para o usuário informado.
        /// </summary>
        /// <remarks>
        /// Realiza o processamento do banco de horas do usuário na data especificada. Requer autenticação.
        /// </remarks>
        /// <response code="200">Processamento realizado com sucesso</response>
        /// <response code="400">Erro ao processar banco de horas</response>
        [Authorize]
        [HttpPost]
        [Route("Processar/{idUsuario}")]
        public async Task<IActionResult> ProcessarBancoHoras(int idUsuario, [FromQuery] DateTime data)
        {
            var resultado = await _bancoHorasService.ProcessarBancoHorasDiarioAsync(idUsuario, data);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Obtém os saldos diários do banco de horas do usuário.
        /// </summary>
        /// <remarks>
        /// Retorna uma lista dos saldos diários do banco de horas para o usuário informado. Requer autenticação.
        /// </remarks>
        /// <response code="200">Saldos diários retornados com sucesso</response>
        /// <response code="400">Erro ao obter saldos diários</response>
        [HttpGet]
        [Authorize]
        [Route("SaldosDiarios/{idUsuario}")]
        public async Task<IActionResult> ObterSaldosDiarios(int idUsuario)
        {
            var resultado = await _bancoHorasService.ObterSaldosUsuarioAsync(idUsuario);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Obtém o saldo atual do banco de horas do usuário.
        /// </summary>
        /// <remarks>
        /// Retorna o saldo atual do banco de horas para o usuário informado. Requer autenticação.
        /// </remarks>
        /// <response code="200">Saldo atual retornado com sucesso</response>
        /// <response code="400">Erro ao obter saldo atual</response>
        [HttpGet]
        [Authorize]
        [Route("Atual/{idUsuario}")]
        public async Task<IActionResult> ObterBancoHorasAtual(int idUsuario)
        {
            var resultado = await _bancoHorasService.ObterBancoHorasAtualAsync(idUsuario);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Obtém as horas trabalhadas por mês do usuário.
        /// </summary>
        /// <remarks>
        /// Retorna a quantidade de horas trabalhadas por mês para o usuário informado. Requer autenticação.
        /// </remarks>
        /// <response code="200">Horas trabalhadas retornadas com sucesso</response>
        /// <response code="400">Erro ao obter horas trabalhadas</response>
        [HttpGet]
        [Authorize]
        [Route("HorasTrabalhadasMes/{idUsuario}")]
        public async Task<IActionResult> ObterHorasTrabalhadasPorMes(int idUsuario)
        {
            var resultado = await _bancoHorasService.ObterHorasTrabalhadasPorMesAsync(idUsuario);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Obtém as horas extras por mês do usuário.
        /// </summary>
        /// <remarks>
        /// Retorna a quantidade de horas extras por mês para o usuário informado. Requer autenticação.
        /// </remarks>
        /// <response code="200">Horas extras retornadas com sucesso</response>
        /// <response code="400">Erro ao obter horas extras</response>
        [HttpGet]
        [Authorize]
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
