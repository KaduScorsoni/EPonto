using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Login;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Método para cadastrar Feriados no sistema
        /// </summary>
        /// <remarks>
        /// Os feriados são para todos os funcionários do sistema. Essa rota precisa de alguns parametros para cadastrar o feriado e retorna se foi sucesso ou não.
        /// </remarks>
        /// <response code="200">Feriado cadastrado com sucesso</response>
        /// <response code="400">Erro ao cadastrar feriado</response>
        [HttpPost]
        [Authorize]
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

        /// <summary>
        /// Método para deletar feriados.
        /// </summary>
        /// <remarks>
        /// Realiza a deleção de feriados de maneira individual pelo identificador do feriado.
        /// </remarks>
        /// <response code="200">Feriado deletado com sucesso</response>
        /// <response code="400">Erro ao deletar o feriado</response>
        [Authorize]
        [HttpDelete]
        [Route("DeletarFeriado/{idFeriado}")]
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

        /// <summary>
        /// Método para listar os feriados cadastrados no sistema.
        /// </summary>
        /// <remarks>
        /// Lista todos os feriados cadastrados no sistema.
        /// </remarks>
        /// <response code="200">Listado com sucesso</response>
        /// <response code="400">Erro ao listar feriados</response>
        [HttpGet]
        [Authorize]
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
        
    }
}
