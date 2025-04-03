using Application.DTOs;
using Application.Interfaces;
using Domain.Entities.Login;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        [Route("RealizarLogin")]
        public ActionResult<LoginDTO> RealizarLogin(LoginModel paramLogin)
        {
            try
            {
                LoginDTO auxResult = _loginService.RealizarLogin(paramLogin);
                if (auxResult.Sucesso)
                    return Ok(auxResult);

                return Unauthorized(new LoginDTO { Sucesso = false, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return Unauthorized(new LoginDTO { Sucesso = false, Mensagem = ex.Message });
            }
        }
    }
}
