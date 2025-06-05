using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities.Comunicado;
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

        [HttpPost]
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
        [HttpDelete]
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
        [HttpGet]
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
