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
        public async Task<ActionResult<bool>> RecuperarSenha(RecuperacaoSenhaModel param)
        {
            try
            {
                bool auxBool = await _loginService.RecuperarSenha(param.email);
                if (auxBool)
                    return Ok(true);

                return BadRequest(false);
            }
            catch (Exception ex)
            {
                return BadRequest(false);
            }
        }
        [HttpPost]
        [Route("ValidaCodigoRecuperacao")]
        public async Task<ActionResult<bool>> ValidaCodigoRecuperacao(ValidaCodigoRecuperacaoModel param)
        {
            try
            {
                bool auxResult = await _loginService.ValidaCodigoRecuperacao(param.codigo, param.email);
                if (auxResult)
                    return Ok(true);

                return BadRequest(false);
            }
            catch (Exception ex)
            {
                return BadRequest(false);
            }
        }
        [HttpPost]
        [Route("AlteraSenhaLogin")]
        public async Task<ActionResult<bool>> AlteraSenhaLogin(AlteraSenhaLoginModel param)
        {
            try
            {
                bool auxResult = await _loginService.AlteraSenhaLogin(param.senha, param.email);
                if (auxResult)
                    return Ok(true);

                return BadRequest(false);
            }
            catch (Exception ex)
            {
                return BadRequest(false);
            }
        }
    }
}
