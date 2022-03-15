using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot.Asset.Model.Resp
{
    public class BaseResp<T>
    {
        public T Data { get; set; }
    }
}
