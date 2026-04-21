using Microsoft.AspNetCore.Mvc;
using miniERPsystem.Services;

namespace miniERPsystem.Controllers
{
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
        
        public IActionResult Production(int IDitemToCraft, decimal quantity)
        {
            var res = _productionService.craftItem(IDitemToCraft, quantity);
            if (res.isSuccessed == false) return BadRequest(res);
            return Ok(res.message);
        }
    }
}
