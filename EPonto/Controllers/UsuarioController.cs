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
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        //private readonly IUsuarioService _usuarioService;

        //public UsuarioController(IUsuarioService usuarioService)
        //{
        //    _usuarioService = usuarioService;
        //}

        //[HttpPost]
        //public async Task<IActionResult> CriarUsuario([FromBody] UsuarioModel usuario)
        //{
        //    var resultado = await _usuarioService.CriarUsuarioAsync(usuario);
        //    if (resultado != null)
        //        return resultado;

        //    return BadRequest("Erro ao criar usuario.");
        //}
    }
}
