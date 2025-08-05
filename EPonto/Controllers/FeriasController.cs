using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeriasController : ControllerBase
    {
        private readonly IFeriasService _FeriasService;
        public FeriasController(IFeriasService FeriasService)
        {
            _FeriasService = FeriasService;
        }

        [HttpPost]
        [Route("CadastrarFerias")]
        public async Task<ActionResult<ResultadoDTO>> CadastrarFerias(FeriasModel paramFerias)
        {
            try
            {
                ResultadoDTO auxResult = await _FeriasService.CadastrarFerias(paramFerias);
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
        [Route("DeletarFerias/{idFerias}")]
        public async Task<ActionResult<ResultadoDTO>> DeletarFerias(int idFerias)
        {
            try
            {
                ResultadoDTO auxResult = await _FeriasService.DeletarFerias(idFerias);
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
                FeriasDTO auxResult = await _FeriasService.ListarFerias(idUsuario);
                if (auxResult.Sucesso)
                    return Ok(auxResult);

                return BadRequest(new FeriasDTO { Sucesso = false, Mensagem = auxResult.Mensagem });
            }
            catch (Exception ex)
            {
                return BadRequest(new FeriasDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
        //[HttpPost]
        //[Route("CadastrarSolicitacaoFerias")]
        //public async Task<ActionResult<ResultadoDTO>> CadastrarSolicitacaoFerias(SolicitacaoFeriasModel paramSolicFerias)
        //{
        //    try
        //    {
        //        ResultadoDTO auxResult = await _FeriasService.CadastrarSolicitacaoFerias(paramFerias);
        //        if (auxResult.Sucesso)
        //            return Ok(auxResult);

        //        return BadRequest(new ResultadoDTO { Sucesso = false, Mensagem = auxResult.Mensagem });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResultadoDTO { Sucesso = false, Mensagem = ex.Message });
        //    }
        //}
    }
}
