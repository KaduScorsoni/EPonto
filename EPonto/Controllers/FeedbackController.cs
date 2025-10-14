using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Entities.Feedback;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost]
        [Authorize]
        [Route("InserirSolicitacao")]
        public async Task<IActionResult> CriarSolicitacao([FromBody] SolicitacaoFeedbackModel solicitacao)
        {
            var resultado = await _feedbackService.CriarSolicitacaoAsync(solicitacao);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpPost]
        [Authorize]
        [Route("InserirFeedback")]
        public async Task<IActionResult> CriarFeedback([FromBody] FeedbackModel feedback)
        {
            var resultado = await _feedbackService.CriarFeedbackAsync(feedback);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpGet]
        [Authorize]
        [Route("ListarSolicitacao")]
        public async Task<IActionResult> ListarTodasSolicitacoes()
        {
            var resultado = await _feedbackService.ListarTodasSolicitacoesAsync();
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpGet]
        [Authorize]
        [Route("ListarFeedback")]
        public async Task<IActionResult> ListarTodosFeedbacks()
        {
            var resultado = await _feedbackService.ListarTodosFeedbacksAsync();
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [Authorize]
        [HttpGet("ListarSolicitacao/{id}")]
        public async Task<IActionResult> ObterSolicitacaoPorId(int id)
        {
            var resultado = await _feedbackService.ObterSolicitacaoPorIdAsync(id);
            if (resultado.Sucesso && resultado.SolicitacaoFeedback != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [Authorize]
        [HttpGet("ListarSolicitacoesUsuario/{idUsuario}")]
        public async Task<IActionResult> ObterSolicitacoesPorUsuario(int idUsuario)
        {
            var resultado = await _feedbackService.ObterSolicitacoesPorUsuarioAsync(idUsuario);

            if (resultado.Sucesso && resultado.SolicitacoesFeedback != null && resultado.SolicitacoesFeedback.Any())
                return Ok(resultado);

            return NotFound(resultado);
        }
        [Authorize]
        [HttpGet("ListarSolicitacoesResponsavel/{idResponsavel}")]
        public async Task<IActionResult> ObterSolicitacoesResponsavel(int idResponsavel)
        {
            var resultado = await _feedbackService.ObterSolicitacoesResponsavelAsync(idResponsavel);

            if (resultado.Sucesso && resultado.SolicitacoesFeedback != null && resultado.SolicitacoesFeedback.Any())
                return Ok(resultado);

            return NotFound(resultado);
        }

        [Authorize]
        [HttpGet("ListarFeedback/{id}")]
        public async Task<IActionResult> ObterFeedbackPorId(int id)
        {
            var resultado = await _feedbackService.ObterFeedbackPorIdAsync(id);
            if (resultado.Sucesso && resultado.Feedback != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [Authorize]
        [HttpPut("AtualizarSolicitacao/{id}")]
        public async Task<IActionResult> AtualizarSolicitacao(int id, [FromBody] SolicitacaoFeedbackModel solicitacao)
        {
            if (solicitacao == null)
            {
                return BadRequest("Dados da solicitação são obrigatórios.");
            }

            solicitacao.IdSolicitacaoFeedback = id;

            var resultado = await _feedbackService.AtualizarSolicitacaoAsync(solicitacao);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [Authorize]
        [HttpGet("ListarFeedbacksUsuario/{idUsuario}")]
        public async Task<IActionResult> ObterFeedbacksPorUsuario(int idUsuario)
        {
            var resultado = await _feedbackService.ObterFeedbacksPorUsuarioAsync(idUsuario);
            if (resultado.Sucesso && resultado.Feedbacks != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [Authorize]
        [HttpDelete("Deletar/{id}")]
        public async Task<IActionResult> ExcluirSolicitacao(int id)
        {
            var resultado = await _feedbackService.ExcluirSolicitacaoAsync(id);
            if (resultado.Sucesso)
                return Ok(resultado);

            return NotFound(resultado);
        }
    }
}
