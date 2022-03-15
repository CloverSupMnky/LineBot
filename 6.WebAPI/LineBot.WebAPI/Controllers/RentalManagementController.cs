using LineBot.Module.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
