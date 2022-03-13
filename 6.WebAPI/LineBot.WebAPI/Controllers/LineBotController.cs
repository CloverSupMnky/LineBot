using LineBot.Asset.Model.Req;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LineBot.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineBotController : ControllerBase
    {
        [HttpPost("[action]")]
        public IActionResult ReplyTextMessage(ReplyTextMessageReq req) 
        {
            return Ok(req.Text);
        }
    }
}
