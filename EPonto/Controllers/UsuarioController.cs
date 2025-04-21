using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Interfaces;
using Application.DTOs;

namespace EPonto.Controllers
{
    [Route("api/Usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [Route("Inserir")]
        public async Task<IActionResult> CriarUsuario([FromBody] UsuarioModel usuario)
        {
            var resultado = await _usuarioService.CriarUsuarioAsync(usuario);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUsuarioPorId(int id)
        {
            var resultado = await _usuarioService.ObterUsuarioPorIdAsync(id);
            if (resultado.Sucesso && resultado.Usuario != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<IActionResult> ListarTodosUsuarios()
        {
            var resultado = await _usuarioService.ListarTodosUsuariosAsync();
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpPut]
        [Route("Atualizar")]
        public async Task<IActionResult> AtualizarUsuario([FromBody] UsuarioModel usuario)
        {
            var resultado = await _usuarioService.AtualizarUsuarioAsync(usuario);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            var resultado = await _usuarioService.ExcluirUsuarioAsync(id);
            if (resultado.Sucesso)
                return Ok(resultado);

            return NotFound(resultado);
        }
    }
}
