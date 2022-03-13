using LineBot.Asset.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot.Asset.Model.LineBot.Base
{
    public class Event
    {
        public EventType Type { get; set; }
        public Message Message { get; set; }
        public long Timestamp { get; set; }
        public Source Source { get; set; }
        public string ReplyToken { get; set; }
        public string Mode { get; set; }
    }
}
