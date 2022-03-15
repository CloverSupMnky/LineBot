using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot.Asset.Model.Resp
{
    public class PersonDTO
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
