using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace LineBot.Entitys.Models
{
    /// <summary>
    /// &#20154;&#21729;&#36039;&#26009;&#34920;
    /// </summary>
    public partial class Person : Entity
    {
        /// <summary>
        /// 人員唯一ID
        /// </summary>
        public Guid PersonId { get; set; }
        /// <summary>
        /// 人員名稱
        /// </summary>
        public string PersonName { get; set; }
    }
}
