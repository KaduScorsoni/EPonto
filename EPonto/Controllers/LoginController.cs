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
        public ActionResult<ResultadoLoginModel> RealizarLogin(LoginModel paramLogin)
        {

            return new ResultadoLoginModel();
        }
    }
}
