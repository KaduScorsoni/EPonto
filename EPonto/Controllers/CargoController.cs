using Application.Interfaces;
using Application.Services;
using Domain.Entities;
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
        [Route("Inserir")]
        public async Task<IActionResult> CriarCargo([FromBody] CargoModel cargo)
        {
            var resultado = await _cargoService.CriarCargoAsync(cargo);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterCargoPorId(int id)
        {
            var resultado = await _cargoService.ObterCargoPorIdAsync(id);
            if (resultado.Sucesso && resultado.Cargo != null)
                return Ok(resultado);

            return NotFound(resultado);
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<IActionResult> ListarTodosCargos()
        {
            var resultado = await _cargoService.ListarTodosCargosAsync();
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

        [HttpPut]
        [Route("Atualizar")]
        public async Task<IActionResult> AtualizarCargo([FromBody] CargoModel cargo)
        {
            var resultado = await _cargoService.AtualizarCargoAsync(cargo);
            if (resultado.Sucesso)
                return Ok(resultado);

            return BadRequest(resultado);
        }

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

