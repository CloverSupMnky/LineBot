using LineBot.Asset.Constant;
using LineBot.Asset.Model.Resp;
using LineBot.Entitys.Models;
using LineBot.Module.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LineBot.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalManagementController : APIBaseController
    {
        private readonly IRentalManagementService rentalManagementService;

        public RentalManagementController(
            IRentalManagementService rentalManagementService) 
        {
            this.rentalManagementService = rentalManagementService;
        }

        [HttpPost("[action]")]
        public IActionResult GetRentDetail() 
        {
            return Success(this.rentalManagementService.GetRentDetail());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteRentItem(RentDetail detail)
        {
            await this.rentalManagementService.DeleteRentItem(detail);

            return Success(true);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> InsertFixedFee(RentFixedFee rentFixedFee)
        {
            await this.rentalManagementService.InsertFixedFee(rentFixedFee);

            return Success(true);
        }

        [HttpPost("[action]")]
        public IActionResult GetFixedSelectList()
        {
            var res = this.rentalManagementService
                .GetSysparamByGroupId(SysparamGroupId.RentFixedFee);

            return Success(res);
        }

        [HttpPost("[action]")]
        public IActionResult GetUtilitySelectList()
        {
            var res = this.rentalManagementService
                .GetSysparamByGroupId(SysparamGroupId.UtilityFee);

            return Success(res);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> InsertUtilityFee(UtilityFee utilityFee)
        {
            await this.rentalManagementService.InsertUtilityFee(utilityFee);

            return Success(true);
        }
    }
}
