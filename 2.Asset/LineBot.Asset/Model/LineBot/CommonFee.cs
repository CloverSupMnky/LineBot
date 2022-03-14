using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot.Asset.Model.LineBot
{
    /// <summary>
    /// 租屋固定、公共費用等共同分擔費用 Model
    /// </summary>
    public class CommonFee
    {
        /// <summary>
        /// 項目名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 項目費用
        /// </summary>
        public decimal Fee { get; set; }
    }
}
