using Application.DTOs;
using Application.Interfaces;
using Domain.Entities.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarioController : ControllerBase
    {
        //private readonly ICalendarioService _calendarioService;
        //public CalendarioController(ICalendarioService calendarioService)
        //{
        //    _calendarioService = calendarioService;
        //}

        //[HttpPost]
        //[Route("BuscaCalendario")]
        //public async Task<ActionResult<CalendarioDTO>> BuscaCalendario()
        //{
        //    try
        //    {
        //        LoginDTO auxResult = await _loginService.RealizarLogin(paramLogin);
        //        if (auxResult.Sucesso)
        //            return Ok(auxResult);

        //        return Unauthorized(new LoginDTO { Sucesso = false, Mensagem = auxResult.Mensagem });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Unauthorized(new LoginDTO { Sucesso = false, Mensagem = ex.Message });
        //    }
        //}
    }
}
