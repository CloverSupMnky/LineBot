using LineBot.Asset.Model.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot.Module.Interface
{
    /// <summary>
    /// 租金管理
    /// </summary>
    public interface IRentalManagementService
    {
        /// <summary>
        /// 取得租金明細
        /// </summary>
        /// <returns></returns>
        IEnumerable<RentDetail> GetRentDetail();

        /// <summary>
        /// 刪除租金項目
        /// </summary>
        Task DeleteRentItem(RentDetail detail);
    }
}
