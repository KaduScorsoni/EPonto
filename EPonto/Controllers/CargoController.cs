using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [Route("api/Cargo")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        private readonly ICargoService _cargoService;
        public CargoController(ICargoService cargoService)
        {
            _cargoService = cargoService;
        }

        /// <summary>
        /// Método para criar cargos dos usuários do sistema.
        /// </summary>
        /// <remarks>
        /// Este método cria cargos para os usuários do sistema, podendo ser alterados depois.
        /// </remarks>
        /// <response code="200">Cargos retornados com sucesso</response>
        /// <response code="400">Erro ao listar cargos</response>
        [HttpPost]
        [Authorize]
        [Route("Inserir")]
        public async Task<IActionResult> CriarCargo([FromBody] CargoModel cargo)
        {
            var resultado = await _cargoService.CriarCargoAsync(cargo);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Método retornar o cargo pelo ID do cargo
        /// </summary>
        /// <remarks>
        /// Este método é utilizado para retornar o cargo selecionado na edição de cargos
        /// </remarks>
        /// <response code="200">Cargo ertornado com sucesso</response>
        /// <response code="400">Erro ao retornar cargo</response>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterCargoPorId(int id)
        {
            var resultado = await _cargoService.ObterCargoPorIdAsync(id);
            if (resultado.Sucesso && resultado.Cargo != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        /// <summary>
        /// Lista todos os cargos.
        /// </summary>
        /// <remarks>
        /// Retorna todos os cargos cadastrados no sistema. Requer autenticação.
        /// </remarks>
        /// <response code="200">Cargos retornados com sucesso</response>
        /// <response code="400">Erro ao listar cargos</response>
        [HttpGet]
        [Authorize]
        [Route("Listar")]
        public async Task<IActionResult> ListarTodosCargos()
        {
            var resultado = await _cargoService.ListarTodosCargosAsync();
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Método para atualizar as informações de cargo.
        /// </summary>
        /// <remarks>
        /// Este método atualiza todas as informações de cargos
        /// </remarks>
        /// <response code="200">Cargo atualizado com sucesso</response>
        /// <response code="400">Erro ao atualizar cargos</response>
        [Authorize]
        [HttpPut("Atualizar/{id}")]
        public async Task<IActionResult> AtualizarCargo(int id, [FromBody] CargoModel cargo)
        {
            if (cargo == null)
            {
                return BadRequest("Dados do cargo são obrigatórios.");
            }

            cargo.IdCargo = id;

            var resultado = await _cargoService.AtualizarCargoAsync(cargo);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        /// <summary>
        /// Método para deletar cargos dos usuários do sistema.
        /// </summary>
        /// <remarks>
        /// Este método deleta logicamente cargos do sistema.
        /// </remarks>
        /// <response code="200">Cargos retornados com sucesso</response>
        /// <response code="400">Erro ao listar cargos</response>
        [Authorize]
        [HttpPut("Deletar/{id}")]
        public async Task<IActionResult> ExcluirCargo(int id)
        {
            var resultado = await _cargoService.ExcluirCargoAsync(id);
            if (resultado.Sucesso)
                return Ok(resultado);

            return NotFound(resultado);
        }
    }
}

