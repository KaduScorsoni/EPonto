using Application.Interfaces;
using Domain.Entities.WhatsApp;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPonto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhatsAppController : ControllerBase
    {
        private readonly IWhatsAppService _whatsAppService;

        public WhatsAppController(IWhatsAppService whatsAppService)
        {
            _whatsAppService = whatsAppService;
        }

        [HttpGet("webhook")]
        public async Task<IActionResult> VerifyWebhook([FromQuery] string hub_mode, 
                                                       [FromQuery] string hub_verify_token, 
                                                       [FromQuery] string hub_challenge)
        {
            var isValid = await _whatsAppService.VerifyWebhook(hub_mode, hub_verify_token, hub_challenge);
            
            if (isValid)
            {
                return Ok(hub_challenge);
            }
            
            return Forbid();
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] WhatsAppWebhookModel webhook)
        {
            var result = await _whatsAppService.ProcessWebhook(webhook);
            
            if (result.Sucesso)
            {
                return Ok();
            }
            
            return BadRequest(result);
        }

        [HttpPost("config")]
        public async Task<IActionResult> SaveConfig([FromBody] WhatsAppConfigModel config)
        {
            var result = await _whatsAppService.SaveConfig(config);
            return Ok(result);
        }

        [HttpGet("config")]
        public async Task<IActionResult> GetConfig()
        {
            var config = await _whatsAppService.GetActiveConfig();
            return Ok(config);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            var result = await _whatsAppService.SendMessage(request.PhoneNumber, request.Message);
            return Ok(result);
        }
    }

    public class SendMessageRequest
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}
