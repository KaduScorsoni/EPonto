using Application.Interfaces;
using Domain.Entities;
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

        [HttpPost]
        [Route("Inserir")]
        public async Task<IActionResult> CriarJornadaTrabalho([FromBody] JornadaTrabalhoModel jornada)
        {
            var resultado = await _jornadaTrabalhoService.CriarJornadaTrabalhoAsync(jornada);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterJornadaTrabalhoPorId(int id)
        {
            var resultado = await _jornadaTrabalhoService.ObterJornadaTrabalhoPorIdAsync(id);
            if (resultado.Sucesso && resultado.Jornada != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<IActionResult> ListarJornadasTrabalho()
        {
            var resultado = await _jornadaTrabalhoService.ListarJornadasTrabalhoAsync();
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

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
