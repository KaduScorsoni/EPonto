using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities.Comunicado;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComunicadoController : ControllerBase
    {
        private readonly IComunicadoService _comunicadoService;
        public ComunicadoController(IComunicadoService comunicadoService)
        {
            _comunicadoService = comunicadoService;
        }

        /// <summary>
        /// Método para cadastrar comunicados para os usuários do sistema.
        /// </summary>
        /// <remarks>
        /// Este método cadastra comunicados, para os administradores exibirem avisos aos usuários.
        /// </remarks>
        /// <response code="200">Comunicados retornados com sucesso</response>
        /// <response code="400">Erro ao cadastrar comunicados</response>
        [HttpPost]
        [Authorize]
        [Route("CadastrarComunicado")]
        public async Task<ActionResult<ResultadoDTO>> CadastrarComunicado(ComunicadoModel param)
        {
            try
            {
                ResultadoDTO auxResult = await _comunicadoService.CadastrarComunicado(param);

                if (auxResult.Sucesso)
                    return Ok(auxResult);

                return BadRequest(new ResultadoDTO { Sucesso = false, Mensagem = auxResult.Mensagem });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultadoDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
        /// <summary>
        /// Método para deletar comunicados do sistema.
        /// </summary>
        /// <remarks>
        /// Este método permite aos administradores deletarem comunicados do sistema.
        /// </remarks>
        /// <response code="200">Comunicado deletado com sucesso</response>
        /// <response code="400">Erro ao deletar comunicados</response>
        [HttpDelete]
        [Authorize]
        [Route("DeletarComunicado")]
        public async Task<ActionResult<ResultadoDTO>> DeletarComunicado(int idComunicado)
        {
            try
            {
                ResultadoDTO auxResult = await _comunicadoService.DeletarComunicado(idComunicado);
                if (auxResult.Sucesso)
                    return Ok(auxResult);

                return BadRequest(new ResultadoDTO { Sucesso = false, Mensagem = auxResult.Mensagem });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultadoDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
        /// <summary>
        /// Método para listar comunicados para os usuários do sistema.
        /// </summary>
        /// <remarks>
        /// Este método retorna os comunicados cadastrados para os usuários.
        /// </remarks>
        /// <response code="200">Comunicados listados com sucesso</response>
        /// <response code="400">Erro ao listar comunicados</response>
        [HttpGet]
        [Authorize]
        [Route("ListarComunicados")]
        public async Task<ActionResult<ComunicadoDTO>> ListarComunicados()
        {
            try
            {
                ComunicadoDTO auxResult = await _comunicadoService.ListarComunicado();
                if (auxResult.Sucesso)
                    return Ok(auxResult);

                return BadRequest(new ComunicadoDTO { Sucesso = false, Mensagem = auxResult.Mensagem });
            }
            catch (Exception ex)
            {
                return BadRequest(new ComunicadoDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
    }
}
