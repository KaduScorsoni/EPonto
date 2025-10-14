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

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterCargoPorId(int id)
        {
            var resultado = await _cargoService.ObterCargoPorIdAsync(id);
            if (resultado.Sucesso && resultado.Cargo != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

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

