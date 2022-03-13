using LineBot.Asset.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot.Asset.Model.LineBot.Base
{
    public class Source
    {
        public SourceType Type { get; set; }
        public string UserId { get; set; }
    }
}
