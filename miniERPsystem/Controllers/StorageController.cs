using Microsoft.AspNetCore.Mvc;
using miniERPsystem.Models;
using miniERPsystem.Services;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetStorage()
        {
            var items = await _context.Storages.ToListAsync();

            return Ok(items);
        }
    }
}