using Microsoft.AspNetCore.Mvc;
using miniERPsystem.Services;

namespace miniERPsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : Controller
    {
        private readonly PurchaseService _purchaseService;
        public PurchaseController(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }
        [HttpPost("order")]
        public IActionResult Order(int id, decimal quantity)
        {
            var res = _purchaseService.BuyItem(id, quantity);
            if (res.isSuccessed == false)
            {
                return BadRequest(res.message);
            }
            return Ok(res);
        }
    }
}
