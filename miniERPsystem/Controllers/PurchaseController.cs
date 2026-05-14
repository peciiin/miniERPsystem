using Microsoft.AspNetCore.Mvc;
using miniERPsystem.Services;

namespace miniERPsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseService _purchaseService;
        public PurchaseController(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }
        [HttpPost("order")]
        public IActionResult Order(int id, decimal quantity, decimal pricePerItem, string? note)
        {
            var res = _purchaseService.BuyItem(id, quantity, pricePerItem, note ?? "Purchase of material");
            if (res.isSuccessed == false)
            {
                return BadRequest(res.message);
            }
            return Ok(res);
        }
    }
}
