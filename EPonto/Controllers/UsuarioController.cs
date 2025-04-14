using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
//using Application.DTOs;
using Application.Interfaces;
using Application.Services;

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
            await _usuarioService.CriarUsuarioAsync(usuario);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUsuarioPorId(int id)
        {
            var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<IActionResult> ListarTodosUsuarios()
        {
            var usuarios = await _usuarioService.ListarTodosUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpPut]
        [Route("Atualizar")]
        public async Task<IActionResult> AtualizarUsuario([FromBody] UsuarioModel usuario)
        {
            var sucesso = await _usuarioService.AtualizarUsuarioAsync(usuario);
            return sucesso ? Ok() : BadRequest("Erro ao atualizar.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            var sucesso = await _usuarioService.ExcluirUsuarioAsync(id);
            return sucesso ? Ok() : NotFound();
        }
    }
}
