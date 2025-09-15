using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Entities.Feedback;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitacaoAusenciaController : ControllerBase
    {
        private readonly ISolicitacaoAusenciaService _solicitacaoAusenciaService;
       

        public SolicitacaoAusenciaController(ISolicitacaoAusenciaService solicitacaoAusenciaService)
        {
            _solicitacaoAusenciaService = solicitacaoAusenciaService;
        }

        [HttpGet]
        [Route("ListarSolicitacaoAusencia")]
        public async Task<IActionResult> ListarTodosFeedbacks()
        {
            var resultado = await _solicitacaoAusenciaService.ListarTodosSolicitacaoAusenciaAsync();
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpGet("ListarSolicitacaoAusencia/{id}")]
        public async Task<IActionResult> ObterSolicitacaoPorId(int id)
        {
            var resultado = await _solicitacaoAusenciaService.ObterSolicitacaoAusenciaPorIdAsync(id);
            if (resultado.Sucesso && resultado.SolicitacaoAusencia != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [HttpGet("ListarSolicitacoesAusenciaUsuario/{idUsuario}")]
        public async Task<IActionResult> ObterSolicitacoesPorUsuario(int idUsuario)
        {
            var resultado = await _solicitacaoAusenciaService.ObterSolicitacoesAusenciaPorUsuarioAsync(idUsuario);

            if (resultado.Sucesso && resultado.Solicitacoes != null && resultado.Solicitacoes.Any())
                return Ok(resultado);

            return NotFound(resultado);
        }

        [HttpPost("InserirSolicitacaoAusencia")]
        public async Task<IActionResult> CriarSolicitacaoAusencia([FromBody] SolicitacaoAusenciaModel solicitacao)
        {
            var resultado = await _solicitacaoAusenciaService.CriarSolicitacaoAusenciaAsync(solicitacao);
            if (resultado.Sucesso)
                return Ok(resultado);
            return BadRequest(resultado);
        }

        [HttpPut("AtualizarSolicitacao/{id}")]
        public async Task<IActionResult> AtualizarSolicitacao(int id, [FromBody] SolicitacaoAusenciaModel solicitacao)
        {
            if (solicitacao == null)
            {
                return BadRequest("Dados da solicitação são obrigatórios.");
            }

            solicitacao.IdSolicitacaoAusencia = id;

            var resultado = await _solicitacaoAusenciaService.AtualizarSolicitacaoAsync(solicitacao);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpDelete("Deletar/{id}")]
        public async Task<IActionResult> ExcluirSolicitacao(int id)
        {
            var resultado = await _solicitacaoAusenciaService.ExcluirSolicitacaoAsync(id);
            if (resultado.Sucesso)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [HttpPut("ResponderSolicitacao/{id}")]
        public async Task<IActionResult> ResponderSolicitacao(int id, [FromQuery] bool aprovar)
        {
            var resultado = await _solicitacaoAusenciaService.ResponderSolicitacaoAsync(id, aprovar);

            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

    }
}