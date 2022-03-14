using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace LineBot.Entitys.Models
{
    /// <summary>
    /// &#31199;&#23627;&#22266;&#23450;&#25903;&#20986;&#34920;
    /// </summary>
    public partial class RentFixedFee : Entity
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int SeqNo { get; set; }
        /// <summary>
        /// 項目 ID
        /// </summary>
        public string ItemId { get; set; }
        /// <summary>
        /// 項目名稱
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 費用
        /// </summary>
        public decimal Fee { get; set; }
    }
}
