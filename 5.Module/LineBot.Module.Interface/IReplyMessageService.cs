using LineBot.Asset.Model.LineBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot.Module.Interface
{
    public interface IReplyMessageService
    {
        /// <summary>
        /// 接收查詢房租訊息
        /// </summary>
        Task QueryRent(WebHookEvent req);
    }
}
