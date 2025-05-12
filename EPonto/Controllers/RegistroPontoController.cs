using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Entities.Ponto;
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
        [Route("ObterRegistrosUsuario")]
        public async Task<IActionResult> ObterRegistrosUsuario(int idUsuario)
        {
            var resultado = await _registroPontoService.ObterRegistrosUsuarioAsync(idUsuario);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
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

        [HttpPost]
        [Route("CriarSolicitacaoAlteracao")]
        public async Task<IActionResult> CriarSolicitacao([FromBody] SolicitacaoAjustePontoModel solicitacao)
        {
            var resultado = await _registroPontoService.CriarSolicitacaoAsync(solicitacao);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpGet]
        [Route("ListarSolicitacoesAlteracao")]
        public async Task<IActionResult> ListarSolicitacoes()
        {
            var resultado = await _registroPontoService.ListarSolicitacoesAsync();
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpGet]
        [Route("ObterSolicitacaoAlteracao/{id}")]
        public async Task<IActionResult> ObterSolicitacaoAlteracao(int id)
        {
            var resultado = await _registroPontoService.ObterSolicitacaoAltercaoPorIdAsync(id);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }


        [HttpPost]
        [Route("ValidarSolicitacao/{idSolicitacao}")]
        public async Task<IActionResult> ValidarSolicitacao(int idSolicitacao, [FromBody] bool aprovado)
        {
            var resultado = await _registroPontoService.AprovarReprovarSolicitacaoAsync(idSolicitacao, aprovado);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }
    }
}
