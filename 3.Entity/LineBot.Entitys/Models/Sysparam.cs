using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace LineBot.Entitys.Models
{
    /// <summary>
    /// &#31995;&#32113;&#21443;&#25976;
    /// </summary>
    public partial class Sysparam : Entity
    {
        /// <summary>
        /// 參數群組代碼
        /// </summary>
        public string GroupId { get; set; }
        /// <summary>
        /// 參數 Id
        /// </summary>
        public string ItemId { get; set; }
        /// <summary>
        /// 參數值
        /// </summary>
        public string ItemValue { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
