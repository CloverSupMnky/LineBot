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
        Task ReplyTextMessage(string text);
    }
}
