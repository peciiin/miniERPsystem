using Microsoft.AspNetCore.Mvc;
using miniERPsystem.Services;
using Microsoft.EntityFrameworkCore;
namespace miniERPsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellController : ControllerBase
    {
        private readonly SellService _sellService;
        public SellController(SellService sellService)
        {
            _sellService = sellService;
        }
        [HttpPost("sell")]
        public async Task<IActionResult> Sell(int id, decimal quantity, decimal pricePerItem, string? note)
        {
            var res = await _sellService.SellItemAsync(id, quantity, pricePerItem, note ?? "Sell of Items");
            if (res.isSuccessed == false)
            {
                return BadRequest(res.message);
            }
            return Ok(res);
        }
    }
}
