using LineBot.Asset.Model.LineBot;
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
        private readonly IPushMessageService pushMessageService;

        public LineBotController(IPushMessageService pushMessageService)
        {
            this.pushMessageService = pushMessageService;
        }

        [HttpPost]
        [ServiceFilter(typeof(VerifySignatureFilter))]
        public async Task<IActionResult> Post(WebHookEvent req)
        {
            await this.pushMessageService.QueryRent(req);

            return Ok();
        }
    }
}
