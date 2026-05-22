using Microsoft.AspNetCore.Mvc;
using miniERPsystem.Services;
using Microsoft.EntityFrameworkCore;
namespace miniERPsystem.Controllers
{
    //fix
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionController : Controller
    {
        private readonly ProductionService _productionService;
        public ProductionController(ProductionService productionService)
        {
            _productionService = productionService;
        }
        [HttpPost("craft")]
        
        public async Task<IActionResult> Production(int IDitemToCraft, decimal quantity)
        {
            var res = await _productionService.CraftItemAsync(IDitemToCraft, quantity);
            if (res.isSuccessed == false) return BadRequest(res);
            return Ok(res.message);
        }
    }
}
