using Application.DTOs;
using Application.Interfaces;
using Domain.Entities.Login;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        [Route("realizarlogin")]
        public async Task<ActionResult<LoginDTO>> RealizarLogin(LoginModel paramLogin)
        {
            try
            {
                LoginDTO auxResult = await _loginService.RealizarLogin(paramLogin);
                if (auxResult.Sucesso)
                    return Ok(auxResult);

                return Unauthorized(new LoginDTO { Sucesso = false, Mensagem = auxResult.Mensagem});
            }
            catch (Exception ex)
            {
                return Unauthorized(new LoginDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
    }
}
