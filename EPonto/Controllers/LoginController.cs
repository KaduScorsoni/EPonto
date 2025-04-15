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

        [HttpPost]
        [Route("RealizarLogin")]
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
        [HttpPost ]
        [Route("RecuperarSenha")]
        public async Task<ActionResult<int>> RecuperarSenha(string email)
        {
            try
            {
                int codigo = await _loginService.RecuperarSenha(email);
                if (codigo > 0)
                    return Ok(codigo);

                return BadRequest(0);
            }
            catch (Exception ex)
            {
                return BadRequest(0);
            }
        }

        //Falta fazer o método para salvar a troca da senha.
        // Deve considerar os parametros de entrada, id usuario, email, senhas e codigo

        
    }
}
