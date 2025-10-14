using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        [Route("Inserir")]
        public async Task<IActionResult> CriarUsuario([FromBody] UsuarioModel usuario)
        {
            var resultado = await _usuarioService.CriarUsuarioAsync(usuario);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUsuarioPorId(int id)
        {
            var resultado = await _usuarioService.ObterUsuarioPorIdAsync(id);
            if (resultado.Sucesso && resultado.Usuario != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [Authorize]
        [HttpGet("Contrato/{id}")]
        public async Task<IActionResult> ObterContratoUsuario(int id)
        {
            var resultado = await _usuarioService.ObterContratoUsuarioAsync(id);
            if (resultado.Sucesso && resultado.Contrato != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [HttpGet]
        [Authorize]
        [Route("Listar")]
        public async Task<IActionResult> ListarTodosUsuarios()
        {
            var resultado = await _usuarioService.ListarTodosUsuariosAsync();
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [Authorize]
        [HttpPut("Atualizar/{id}")]
        public async Task<IActionResult> AtualizarUsuario(int id, [FromBody] UsuarioModel usuario)
        {
            if (usuario == null)
            {
                return BadRequest("Dados do usuário são obrigatórios.");
            }

            usuario.IdUsuario = id;

            var resultado = await _usuarioService.AtualizarUsuarioAsync(usuario);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [Authorize]
        [HttpPut("Deletar/{id}")]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            var resultado = await _usuarioService.ExcluirUsuarioAsync(id);
            if (resultado.Sucesso)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [Authorize]
        [HttpGet("Hierarquia")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioModel>), 200)]
        public async Task<IActionResult> ObterHierarquia()
        {
            var arvore = await _usuarioService.ObterHierarquia();
            return Ok(arvore);
        }
    }
}
