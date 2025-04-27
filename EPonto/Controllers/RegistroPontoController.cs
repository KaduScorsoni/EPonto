using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [Route("api/RegistroPonto")]
    [ApiController]
    public class RegistroPontoController : ControllerBase
    {
        private readonly IRegistroPontoService _registroPontoService;

        public RegistroPontoController(IRegistroPontoService registroPontoService)
        {
            _registroPontoService = registroPontoService;
        }

        [HttpPost]
        [Route("Inserir")]
        public async Task<IActionResult> CriarRegistroPonto([FromBody] RegistroPontoModel ponto)
        {
            var resultado = await _registroPontoService.CriarRegistroPontoAsync(ponto);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterRegistroPontoId(int id)
        {
            var resultado = await _registroPontoService.ObterRegistroPontoIdAsync(id);
            if (resultado.Sucesso && resultado.RegistroPonto != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<IActionResult> ListarTodosRegistrosPonto()
        {
            var resultado = await _registroPontoService.ListarTodosRegistrosPontoAsync();
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpPut]
        [Route("Atualizar")]
        public async Task<IActionResult> AtualizarUsuario([FromBody] RegistroPontoModel ponto)
        {
            var resultado = await _registroPontoService.AtualizarRegistroPontoAsync(ponto);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirRegistroPonto(int id)
        {
            var resultado = await _registroPontoService.ExcluirRegistroPontoAsync(id);
            if (resultado.Sucesso)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [HttpPost]
        [Route("Validar")]
        public async Task<IActionResult> ValidarMes(int idUsuario, int anoReferencia, int mesReferencia, int statusValidacao)
        {
            var resultado = await _registroPontoService.ValidarMesAsync(idUsuario, anoReferencia, mesReferencia, statusValidacao);
            if (resultado.Sucesso)
                return Ok(resultado);
            return BadRequest(resultado);
        }
    }
}
