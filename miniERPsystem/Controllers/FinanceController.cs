using Microsoft.AspNetCore.Mvc;
using miniERPsystem.Models;

namespace miniERPsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly MiniErpsystemContext _db;
        public FinanceController(MiniErpsystemContext db)
        {
            _db = db;
        }
        [HttpGet("balance")]
        public IActionResult GetFinaceBalance()
        {
            var totalMoney = _db.Finances.Sum(x => x.TotalPrice);
            var totalProfit = _db.Finances.Where(x => x.TotalPrice > 0).Sum(x => x.TotalPrice);
            var totalSpendings = _db.Finances.Where(x => x.TotalPrice < 0).Sum(x => x.TotalPrice);
            var totalTransactions = _db.Finances.Count();

            return Ok(new{
                TotalMoney = totalMoney,
                TotalProfit = totalProfit,
                TotalSpendings = totalSpendings,
                Currency = "CZK",
                TotalTransactions = totalTransactions
            });
        }

        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            var history = _db.Finances
                .OrderByDescending(f => f.Created)
                .ToList();

            return Ok(history);
        }
    }
}
