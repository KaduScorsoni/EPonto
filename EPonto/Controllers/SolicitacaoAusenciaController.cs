using Application.DTOs;
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
    public class SolicitacaoAusenciaController : ControllerBase
    {
        private readonly ISolicitacaoAusenciaService _solicitacaoAusenciaService;
       

        public SolicitacaoAusenciaController(ISolicitacaoAusenciaService solicitacaoAusenciaService)
        {
            _solicitacaoAusenciaService = solicitacaoAusenciaService;
        }

        [HttpGet]
        [Authorize]
        [Route("ListarSolicitacaoAusencia")]
        public async Task<IActionResult> ListarTodosFeedbacks()
        {
            var resultado = await _solicitacaoAusenciaService.ListarTodosSolicitacaoAusenciaAsync();
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [Authorize]
        [HttpGet("ListarSolicitacaoAusencia/{id}")]
        public async Task<IActionResult> ObterSolicitacaoPorId(int id)
        {
            var resultado = await _solicitacaoAusenciaService.ObterSolicitacaoAusenciaPorIdAsync(id);
            if (resultado.Sucesso && resultado.SolicitacaoAusencia != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [Authorize]
        [HttpGet("ListarSolicitacoesAusenciaUsuario/{idUsuario}")]
        public async Task<IActionResult> ObterSolicitacoesPorUsuario(int idUsuario)
        {
            var resultado = await _solicitacaoAusenciaService.ObterSolicitacoesAusenciaPorUsuarioAsync(idUsuario);

            if (resultado.Sucesso && resultado.Solicitacoes != null && resultado.Solicitacoes.Any())
                return Ok(resultado);

            return NotFound(resultado);
        }

        [Authorize]
        [HttpPost("InserirSolicitacaoAusencia")]
        [Consumes("multipart/form-data")]
        [RequestFormLimits(MultipartBodyLengthLimit = 50_000_000)]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> CriarSolicitacaoAusencia(
            [FromForm] SolicitacaoAusenciaUpsertDto input,
            [FromForm] string[]? camposAtivos,
            [FromServices] ICloudStorage storage)
        {
            if (input == null) return BadRequest("Payload inválido.");

            var ativos = new HashSet<string>(
                (camposAtivos ?? Array.Empty<string>())
                .Select(s => s?.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))!
                .Select(s => s!.ToLowerInvariant())
            );

            bool CampoAtivo(string nome) => ativos.Count == 0 || ativos.Contains(nome.ToLowerInvariant());

            if (input.IdUsuario <= 0)
                return BadRequest("IdUsuario inválido.");

            if (CampoAtivo(nameof(input.DataInicioAusencia)) && CampoAtivo(nameof(input.DataFimAusencia))
                && input.DataInicioAusencia.HasValue && input.DataFimAusencia.HasValue)
            {
                if (input.DataFimAusencia < input.DataInicioAusencia)
                    return BadRequest("DataFimAusencia não pode ser anterior à DataInicioAusencia.");
            }

            string? url = null;
            if (CampoAtivo(nameof(input.Arquivo)) && input.Arquivo != null && input.Arquivo.Length > 0)
            {
                var permitidos = new[]
                {
            "application/pdf","image/jpeg","image/png","image/webp",
            "application/msword","application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };
                if (!permitidos.Contains(input.Arquivo.ContentType))
                    return BadRequest("Tipo de arquivo não permitido.");

                url = await storage.UploadAsync(
                    input.Arquivo,
                    fileNameHint: $"solicitacao_{input.IdUsuario}_{input.Arquivo.FileName}"
                );
            }

            var solicitacao = new Domain.Entities.SolicitacaoAusenciaModel
            {
                IdUsuario = input.IdUsuario,
                MensagemSolicitacao = CampoAtivo(nameof(input.MensagemSolicitacao)) ? input.MensagemSolicitacao : null,
                DataInicioAusencia = CampoAtivo(nameof(input.DataInicioAusencia)) && input.DataInicioAusencia.HasValue
                    ? input.DataInicioAusencia.Value
                    : default,
                DataFimAusencia = CampoAtivo(nameof(input.DataFimAusencia)) && input.DataFimAusencia.HasValue
                    ? input.DataFimAusencia.Value
                    : default, 
                LinkArquivo = CampoAtivo(nameof(input.Arquivo)) ? url : null
            };

            var resultado = await _solicitacaoAusenciaService.CriarSolicitacaoAusenciaAsync(solicitacao);
            return resultado.Sucesso ? Ok(resultado) : BadRequest(resultado);
        }


        [Authorize]
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

        [Authorize]
        [HttpDelete("Deletar/{id}")]
        public async Task<IActionResult> ExcluirSolicitacao(int id)
        {
            var resultado = await _solicitacaoAusenciaService.ExcluirSolicitacaoAsync(id);
            if (resultado.Sucesso)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [Authorize]
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