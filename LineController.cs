using Line.Messaging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LineBotProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineController : ControllerBase
    {
        private readonly ILineMessagingClient _lineMessagingClient;

        public LineController(ILineMessagingClient lineMessagingClient)
        {
            _lineMessagingClient = lineMessagingClient;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Post([FromBody] Events.WebhookEvent request)
        {
            var events = request.Events;
            var channelSecret = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["LineSettings:ChannelSecret"];
            var bot = new LineBotApp(_lineMessagingClient, channelSecret);
            await bot.RunAsync(events);

            return Ok();
        }
    }
}