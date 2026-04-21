using Microsoft.AspNetCore.Mvc;
using miniERPsystem.Models;
using miniERPsystem.Services;

namespace miniERPsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly MiniErpsystemContext _context;
        

        public StorageController(MiniErpsystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetStorage()
        {
            var items = _context.Storages.ToList();

            return Ok(items);
        }
    }
}