using LineBot.Asset.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot.Asset.Model.Resp
{
    /// <summary>
    /// 租金明細
    /// </summary>
    public class RentDetail
    {
        /// <summary>
        /// 費用類型
        /// </summary>
        public RentType Type { get; set; }

        /// <summary>
        /// 費用名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 費用類型序號
        /// </summary>
        public int SeqNo { get; set; }

        /// <summary>
        /// 費用
        /// </summary>
        public string Fee { get; set; }
    }
}
