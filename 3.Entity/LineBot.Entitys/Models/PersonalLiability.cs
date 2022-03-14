using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace LineBot.Entitys.Models
{
    /// <summary>
    /// &#20154;&#21729;&#36000;&#20661;&#34920;
    /// </summary>
    public partial class PersonalLiability : Entity
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int SeqNo { get; set; }
        /// <summary>
        /// 債主ID
        /// </summary>
        public Guid CreditorId { get; set; }
        /// <summary>
        /// 欠債人ID
        /// </summary>
        public Guid DebtorId { get; set; }
        /// <summary>
        /// 項目名稱
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 金額
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// 產生債務時間
        /// </summary>
        public int CreateOn { get; set; }
        /// <summary>
        /// 是否已結清
        /// </summary>
        public bool IsClosed { get; set; }
        /// <summary>
        /// 結清債務時間
        /// </summary>
        public int? ClosedOn { get; set; }
    }
}
