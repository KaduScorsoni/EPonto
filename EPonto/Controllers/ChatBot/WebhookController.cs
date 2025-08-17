using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPonto.Controllers.ChatBot
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private const string VerifyToken = "MEU_TOKEN_DE_VERIFICACAO";

        // GET - Verificação de webhook
        [HttpGet]
        public IActionResult Verify([FromQuery] string hub_mode, [FromQuery] string hub_challenge, [FromQuery] string hub_verify_token)
        {
            if (hub_mode == "subscribe" && hub_verify_token == VerifyToken)
                return Ok(hub_challenge);

            return Unauthorized();
        }

        // POST - Recebendo mensagens
        [HttpPost]
        public async Task<IActionResult> Receive([FromBody] object data)
        {
            Console.WriteLine("Mensagem recebida: " + data.ToString());
            // Aqui você pode desserializar o JSON e processar a mensagem
            return Ok();
        }
    }
}
