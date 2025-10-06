using Application.DTOs;
using Application.Interfaces;
using Domain.Entities.Perfil;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly IPerfilService _perfilService;
        public PerfilController(IPerfilService perfilService)
        {
            _perfilService = perfilService;
        }

        [HttpPost]
        [Route("CadastrarPerfil")]
        public async Task<ActionResult<ResultadoDTO>> CadastrarPerfil(PerfilModel paramPerfil)
        {
            try
            {
                var result = await _perfilService.CadastrarPerfil(paramPerfil);
                if (result.Sucesso)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultadoDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }

        [HttpGet]
        [Route("ListarPerfis")]
        public async Task<ActionResult<PerfilDTO>> ListarPerfis()
        {
            try
            {
                var result = await _perfilService.ListarPerfis();
                if (result.Sucesso)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new PerfilDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
        [HttpGet]
        [Route("ListarPerfil")]
        public async Task<ActionResult<PerfilDTO>> ListarPerfil(int idPerfil)
        {
            try
            {
                var result = await _perfilService.ListarPerfil(idPerfil);
                if (result.Sucesso)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new PerfilDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
        [HttpPut]
        [Route("EditarPerfil")]
        public async Task<ActionResult<ResultadoDTO>> EditarPerfil(PerfilModel paramPerfil)
        {
            try
            {
                var result = await _perfilService.EditarPerfil(paramPerfil);
                if (result.Sucesso)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultadoDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }

        [HttpDelete]
        [Route("RemoverPerfil/{idPerfil}")]
        public async Task<ActionResult<ResultadoDTO>> RemoverPerfil(int idPerfil)
        {
            try
            {
                var result = await _perfilService.RemoverPerfil(idPerfil);
                if (result.Sucesso)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultadoDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
        [HttpPost]
        [Route("CadastrarVinculoPerfilUsuario")]
        public async Task<ActionResult<ResultadoDTO>> CadastrarVinculoPerfilUsuario(VinculoPerfilUsuario param)
        {
            try
            {
                var result = await _perfilService.CadastrarVinculoPerfilUsuario(param);
                if (result.Sucesso)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultadoDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
    }
}
