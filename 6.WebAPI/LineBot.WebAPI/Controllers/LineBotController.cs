using LineBot.Asset.Model.Req;
using LineBot.Module.Interface;
using LineBot.WebAPI.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LineBot.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineBotController : ControllerBase
    {
        private readonly IReplyMessageService replyMessageService;

        public LineBotController(IReplyMessageService replyMessageService)
        {
            this.replyMessageService = replyMessageService;
        }

        [HttpPost("[action]")]
        [ServiceFilter(typeof(VerifySignatureFilter))]
        public async Task<IActionResult> ReplyTextMessage(ReplyTextMessageReq req) 
        {
            await this.replyMessageService.ReplyTextMessage(req.Text);

            return Ok(req.Text);
        }
    }
}
