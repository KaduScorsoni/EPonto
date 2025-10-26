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

        /// <summary>
        /// Cria uma nova solicitação de feedback.
        /// </summary>
        /// <remarks>
        /// Recebe os dados da solicitação via corpo da requisição. Requer autenticação.
        /// </remarks>
        /// <response code="200">Solicitação criada com sucesso</response>
        /// <response code="400">Erro ao criar solicitação</response>
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

        /// <summary>
        /// Cria um novo feedback.
        /// </summary>
        /// <remarks>
        /// Recebe os dados do feedback via corpo da requisição. Requer autenticação.
        /// </remarks>
        /// <response code="200">Feedback criado com sucesso</response>
        /// <response code="400">Erro ao criar feedback</response>
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

        /// <summary>
        /// Lista todas as solicitações de feedback.
        /// </summary>
        /// <remarks>
        /// Retorna todas as solicitações cadastradas. Requer autenticação.
        /// </remarks>
        /// <response code="200">Solicitações retornadas com sucesso</response>
        /// <response code="400">Erro ao listar solicitações</response>
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

        /// <summary>
        /// Lista todos os feedbacks.
        /// </summary>
        /// <remarks>
        /// Retorna todos os feedbacks cadastrados. Requer autenticação.
        /// </remarks>
        /// <response code="200">Feedbacks retornados com sucesso</response>
        /// <response code="400">Erro ao listar feedbacks</response>
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

        /// <summary>
        /// Obtém uma solicitação de feedback pelo identificador.
        /// </summary>
        /// <remarks>
        /// Retorna a solicitação de feedback correspondente ao id informado. Requer autenticação.
        /// </remarks>
        /// <response code="200">Solicitação encontrada</response>
        /// <response code="404">Solicitação não encontrada</response>
        [Authorize]
        [HttpGet("ListarSolicitacao/{id}")]
        public async Task<IActionResult> ObterSolicitacaoPorId(int id)
        {
            var resultado = await _feedbackService.ObterSolicitacaoPorIdAsync(id);
            if (resultado.Sucesso && resultado.SolicitacaoFeedback != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        /// <summary>
        /// Obtém todas as solicitações de feedback de um usuário.
        /// </summary>
        /// <remarks>
        /// Retorna todas as solicitações de feedback do usuário informado. Requer autenticação.
        /// </remarks>
        /// <response code="200">Solicitações encontradas</response>
        /// <response code="404">Nenhuma solicitação encontrada</response>
        [Authorize]
        [HttpGet("ListarSolicitacoesUsuario/{idUsuario}")]
        public async Task<IActionResult> ObterSolicitacoesPorUsuario(int idUsuario)
        {
            var resultado = await _feedbackService.ObterSolicitacoesPorUsuarioAsync(idUsuario);

            if (resultado.Sucesso && resultado.SolicitacoesFeedback != null && resultado.SolicitacoesFeedback.Any())
                return Ok(resultado);

            return NotFound(resultado);
        }

        /// <summary>
        /// Obtém todas as solicitações de feedback de um responsável.
        /// </summary>
        /// <remarks>
        /// Retorna todas as solicitações de feedback do responsável informado. Requer autenticação.
        /// </remarks>
        /// <response code="200">Solicitações encontradas</response>
        /// <response code="404">Nenhuma solicitação encontrada</response>
        [Authorize]
        [HttpGet("ListarSolicitacoesResponsavel/{idResponsavel}")]
        public async Task<IActionResult> ObterSolicitacoesResponsavel(int idResponsavel)
        {
            var resultado = await _feedbackService.ObterSolicitacoesResponsavelAsync(idResponsavel);

            if (resultado.Sucesso && resultado.SolicitacoesFeedback != null && resultado.SolicitacoesFeedback.Any())
                return Ok(resultado);

            return NotFound(resultado);
        }

        /// <summary>
        /// Obtém um feedback pelo identificador.
        /// </summary>
        /// <remarks>
        /// Retorna o feedback correspondente ao id informado. Requer autenticação.
        /// </remarks>
        /// <response code="200">Feedback encontrado</response>
        /// <response code="404">Feedback não encontrado</response>
        [Authorize]
        [HttpGet("ListarFeedback/{id}")]
        public async Task<IActionResult> ObterFeedbackPorId(int id)
        {
            var resultado = await _feedbackService.ObterFeedbackPorIdAsync(id);
            if (resultado.Sucesso && resultado.Feedback != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        /// <summary>
        /// Atualiza uma solicitação de feedback.
        /// </summary>
        /// <remarks>
        /// Atualiza os dados da solicitação de feedback informada. Requer autenticação.
        /// </remarks>
        /// <response code="200">Solicitação atualizada com sucesso</response>
        /// <response code="400">Erro ao atualizar solicitação</response>
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

        /// <summary>
        /// Obtém todos os feedbacks de um usuário.
        /// </summary>
        /// <remarks>
        /// Retorna todos os feedbacks do usuário informado. Requer autenticação.
        /// </remarks>
        /// <response code="200">Feedbacks encontrados</response>
        /// <response code="404">Nenhum feedback encontrado</response>
        [Authorize]
        [HttpGet("ListarFeedbacksUsuario/{idUsuario}")]
        public async Task<IActionResult> ObterFeedbacksPorUsuario(int idUsuario)
        {
            var resultado = await _feedbackService.ObterFeedbacksPorUsuarioAsync(idUsuario);
            if (resultado.Sucesso && resultado.Feedbacks != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        /// <summary>
        /// Exclui uma solicitação de feedback.
        /// </summary>
        /// <remarks>
        /// Remove a solicitação de feedback correspondente ao id informado. Requer autenticação.
        /// </remarks>
        /// <response code="200">Solicitação excluída com sucesso</response>
        /// <response code="404">Solicitação não encontrada</response>
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
