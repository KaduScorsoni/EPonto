using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Feriado_e_Ferias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

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

        /// <summary>
        /// Método para cadastrar Ferias dos funcionários
        /// </summary>
        /// <remarks>
        /// Essa rota precisa de alguns parametros para cadastrar as férias e retorna se foi sucesso ou não.
        /// </remarks>
        /// <response code="200">Lista de produtos retornada com sucesso</response>
        /// <response code="404">Nenhum produto encontrado</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("CadastrarFerias")]
        [SwaggerRequestExample(typeof(FeriasModel), typeof(FeriasRequestExampleModel))]
        //[SwaggerResponseExample(StatusCodes.Status200OK, typeof(FeriasResponse))]
        //[SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProdutoExample))]
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
                return NotFound(new ResultadoDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Remove o registro de férias pelo identificador.
        /// </summary>
        /// <remarks>
        /// Remove o registro de férias do funcionário pelo id informado. Requer autenticação.
        /// </remarks>
        /// <response code="200">Férias removidas com sucesso</response>
        /// <response code="400">Erro ao remover férias</response>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("RemoverFerias/{id}")]
        public async Task<ActionResult<ResultadoDTO>> RemoverFerias(int id)
        {
            try
            {
                ResultadoDTO auxResult = await _FeriasService.DeletarFerias(id);
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
        /// Remove o registro de férias pelo identificador.
        /// </summary>
        /// <remarks>
        /// Exclui o registro de férias do usuário informado. Retorna sucesso ou mensagem de erro.
        /// </remarks>
        /// <response code="200">Férias removidas com sucesso</response>
        /// <response code="400">Erro ao remover férias</response>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Lista as férias cadastradas de um usuário.
        /// </summary>
        /// <remarks>
        /// Retorna todas as férias do usuário informado. Se não informado, retorna de todos.
        /// </remarks>
        /// <response code="200">Lista de férias retornada com sucesso</response>
        /// <response code="400">Erro ao listar férias</response>
        [HttpGet]
        [Authorize]
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

        /// <summary>
        /// Cadastra uma solicitação de férias.
        /// </summary>
        /// <remarks>
        /// Recebe os dados da solicitação via SolicitacaoFeriasModel e registra no sistema. Retorna sucesso ou mensagem de erro.
        /// </remarks>
        /// <response code="200">Solicitação cadastrada com sucesso</response>
        /// <response code="400">Erro ao cadastrar solicitação</response>
        [HttpPost]
        [Authorize]
        [Route("CadastrarSolicitacaoFerias")]
        public async Task<ActionResult<ResultadoDTO>> CadastrarSolicitacaoFerias(SolicitacaoFeriasModel paramSolicFerias)
        {
            try
            {
                ResultadoDTO auxResult = await _FeriasService.CadastrarSolicitacaoFerias(paramSolicFerias);
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
        /// Lista as solicitações de férias de um usuário.
        /// </summary>
        /// <remarks>
        /// Retorna todas as solicitações de férias do usuário informado. Se não informado, retorna de todos.
        /// </remarks>
        /// <response code="200">Lista de solicitações retornada com sucesso</response>
        /// <response code="400">Erro ao listar solicitações</response>
        [HttpGet]
        [Authorize]
        [Route("ListarSolicitacoesFerias")]
        public async Task<ActionResult<SolicitacaoFeriasDTO>> ListarSolicitacoesFerias(int? idUsuario = null)
        {
            try
            {
                SolicitacaoFeriasDTO auxResult = await _FeriasService.ListarSolicitacoesFerias(idUsuario);
                if (auxResult.Sucesso)
                    return Ok(auxResult);

                return BadRequest(new SolicitacaoFeriasDTO { Sucesso = false, Mensagem = auxResult.Mensagem });
            }
            catch (Exception ex)
            {
                return BadRequest(new SolicitacaoFeriasDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Retorna o saldo de férias do usuário.
        /// </summary>
        /// <remarks>
        /// Consulta o saldo de férias disponível para o usuário informado.
        /// </remarks>
        /// <response code="200">Saldo de férias retornado com sucesso</response>
        /// <response code="400">Erro ao consultar saldo de férias</response>
        [HttpGet]
        [Authorize]
        [Route("RetornaSaldoFerias")]
        public async Task<ActionResult<SaldoFeriasDTO>> RetornaSaldoFerias(int? idUsuario = null)
        {
            try
            {
                SaldoFeriasDTO auxResult = await _FeriasService.RetornaSaldoFerias(idUsuario);
                if (auxResult.Sucesso)
                    return Ok(auxResult);

                return BadRequest(new SaldoFeriasDTO { Sucesso = false, Mensagem = auxResult.Mensagem });
            }
            catch (Exception ex)
            {
                return BadRequest(new SaldoFeriasDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza a situação de uma solicitação de férias.
        /// </summary>
        /// <remarks>
        /// Altera o status da solicitação de férias informada. Retorna sucesso ou mensagem de erro.
        /// </remarks>
        /// <response code="200">Solicitação atualizada com sucesso</response>
        /// <response code="400">Erro ao atualizar solicitação</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("AtualizaSolicitacaoFerias")]
        public async Task<ActionResult<ResultadoDTO>> AtualizaSolicitacaoFerias(int? idSolicitacao = null, int? indSituacao = null)
        {
            try
            {
                ResultadoDTO auxResult = await _FeriasService.AtualizaSolicitacaoFerias(idSolicitacao, indSituacao);
                if (auxResult.Sucesso)
                    return Ok(auxResult);

                return BadRequest(new SolicitacaoFeriasDTO { Sucesso = false, Mensagem = auxResult.Mensagem });
            }
            catch (Exception ex)
            {
                return BadRequest(new SolicitacaoFeriasDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
    }
}
