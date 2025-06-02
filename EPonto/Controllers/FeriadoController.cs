using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Login;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [ApiController]
    [Route("api/feriado")]
    public class FeriadoController : ControllerBase
    {
        private readonly IFeriadoService _feriadoService;
        public FeriadoController(IFeriadoService feriadoService)
        {
            _feriadoService = feriadoService;
        }

        [HttpPost]
        [Route("CadastrarFeriado")]
        public async Task<ActionResult<ResultadoDTO>> CadastrarFeriado(FeriadoModel paramFeriado)
        {
            try
            {
                ResultadoDTO auxResult = await _feriadoService.CadastrarFeriado(paramFeriado);
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
        [Route("DeletarFeriado")]
        public async Task<ActionResult<ResultadoDTO>> DeletarFeriado(int idFeriado)
        {
            try
            {
                ResultadoDTO auxResult = await _feriadoService.DeletarFeriado(idFeriado);
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
        [Route("ListarFeriados")]
        public async Task<ActionResult<FeriadoDTO>> ListarFeriados()
        {
            try
            {
                FeriadoDTO auxResult = await _feriadoService.ListarFeriados();
                if (auxResult.Sucesso)
                    return Ok(auxResult);

                return BadRequest(new FeriadoDTO { Sucesso = false, Mensagem = auxResult.Mensagem });
            }
            catch (Exception ex)
            {
                return BadRequest(new FeriadoDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
        [HttpPost]
        [Route("CadastrarFerias")]
        public async Task<ActionResult<ResultadoDTO>> CadastrarFerias(FeriasModel paramFerias)
        {
            try
            {
                ResultadoDTO auxResult = await _feriadoService.CadastrarFerias(paramFerias);
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
        [Route("DeletarFerias")]
        public async Task<ActionResult<ResultadoDTO>> DeletarFerias(int idFerias)
        {
            try
            {
                ResultadoDTO auxResult = await _feriadoService.DeletarFerias(idFerias);
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
        [Route("ListarFerias")]
        public async Task<ActionResult<FeriasDTO>> ListarFerias(int? idUsuario = null)
        {
            try
            {
                FeriasDTO auxResult = await _feriadoService.ListarFerias(idUsuario);
                if (auxResult.Sucesso)
                    return Ok(auxResult);

                return BadRequest(new FeriasDTO { Sucesso = false, Mensagem = auxResult.Mensagem });
            }
            catch (Exception ex)
            {
                return BadRequest(new FeriasDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
    }
}
