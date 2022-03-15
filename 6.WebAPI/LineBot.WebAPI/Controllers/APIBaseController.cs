using LineBot.Asset.Model.Resp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LineBot.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIBaseController : ControllerBase
    {
        protected internal virtual IActionResult Success<T>(T content)
        {
            var resp = new BaseResp<T>()
            {
                Data = content
            };

            return Ok(resp);
        }
    }
}
