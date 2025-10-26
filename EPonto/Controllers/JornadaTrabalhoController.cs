using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JornadaTrabalhoController : ControllerBase
    {
        private readonly IJornadaTrabalhoService _jornadaTrabalhoService;
        public JornadaTrabalhoController(IJornadaTrabalhoService jornadaTrabalhoService)
        {
            _jornadaTrabalhoService = jornadaTrabalhoService;
        }

        /// <summary>
        /// Cria uma nova jornada de trabalho.
        /// </summary>
        /// <remarks>
        /// Recebe os dados da jornada via corpo da requisição. Requer autenticação.
        /// </remarks>
        /// <response code="200">Jornada criada com sucesso</response>
        /// <response code="400">Erro ao criar jornada</response>
        [HttpPost]
        [Authorize]
        [Route("Inserir")]
        public async Task<IActionResult> CriarJornadaTrabalho([FromBody] JornadaTrabalhoModel jornada)
        {
            var resultado = await _jornadaTrabalhoService.CriarJornadaTrabalhoAsync(jornada);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Obtém uma jornada de trabalho pelo identificador.
        /// </summary>
        /// <remarks>
        /// Retorna a jornada de trabalho correspondente ao id informado. Requer autenticação.
        /// </remarks>
        /// <response code="200">Jornada encontrada</response>
        /// <response code="404">Jornada não encontrada</response>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterJornadaTrabalhoPorId(int id)
        {
            var resultado = await _jornadaTrabalhoService.ObterJornadaTrabalhoPorIdAsync(id);
            if (resultado.Sucesso && resultado.Jornada != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        /// <summary>
        /// Lista todas as jornadas de trabalho.
        /// </summary>
        /// <remarks>
        /// Retorna todas as jornadas cadastradas. Requer autenticação.
        /// </remarks>
        /// <response code="200">Jornadas retornadas com sucesso</response>
        /// <response code="400">Erro ao listar jornadas</response>
        [HttpGet]
        [Authorize]
        [Route("Listar")]
        public async Task<IActionResult> ListarJornadasTrabalho()
        {
            var resultado = await _jornadaTrabalhoService.ListarJornadasTrabalhoAsync();
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Atualiza uma jornada de trabalho.
        /// </summary>
        /// <remarks>
        /// Atualiza os dados da jornada de trabalho informada. Requer autenticação.
        /// </remarks>
        /// <response code="200">Jornada atualizada com sucesso</response>
        /// <response code="400">Erro ao atualizar jornada</response>
        [Authorize]
        [HttpPut("Atualizar/{id}")]
        public async Task<IActionResult> AtualizarJornadaTrabalho(int id, [FromBody] JornadaTrabalhoModel jornada)
        {
            if (jornada == null)
            {
                return BadRequest("Dados do jornada são obrigatórios.");
            }

            jornada.IdJornada = id;

            var resultado = await _jornadaTrabalhoService.AtualizarJornadaTrabalhoAsync(jornada);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Exclui uma jornada de trabalho.
        /// </summary>
        /// <remarks>
        /// Remove a jornada de trabalho correspondente ao id informado. Requer autenticação.
        /// </remarks>
        /// <response code="200">Jornada excluída com sucesso</response>
        /// <response code="404">Jornada não encontrada</response>
        [Authorize]
        [HttpPut("Deletar/{id}")]
        public async Task<IActionResult> ExcluirJornadaTrabalho(int id)
        {
            var resultado = await _jornadaTrabalhoService.ExcluirJornadaTrabalhoAsync(id);
            if (resultado.Sucesso)
                return Ok(resultado);

            return NotFound(resultado);
        }
    }
}
