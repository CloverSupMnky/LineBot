using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace LineBot.Entitys.Models
{
    /// <summary>
    /// &#31199;&#23627;&#20844;&#20849;&#36027;&#29992;&#34920;
    /// </summary>
    public partial class UtilityFee : Entity
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
        /// <summary>
        /// 產生費用的時間
        /// </summary>
        public int CreateOn { get; set; }
        /// <summary>
        /// 結清費用的時間
        /// </summary>
        public int? ClosedOn { get; set; }
        /// <summary>
        /// 是否已結清
        /// </summary>
        public bool IsClosed { get; set; }
    }
}
