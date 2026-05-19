using Microsoft.AspNetCore.Mvc;
using miniERPsystem.Models;
using miniERPsystem.Services;

namespace miniERPsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly MiniErpsystemContext _db;
        private readonly FinanceService _financeService;
        public FinanceController(MiniErpsystemContext db, FinanceService financeService)
        {
            _db = db;
            _financeService = financeService;
        }
        [HttpGet("balance")]
        public IActionResult GetFinanceBalance()
        {
            var totalMoney = _db.Finances.Sum(x => x.TotalPrice);
            var totalProfit = _db.Finances.Where(x => x.TotalPrice > 0).Sum(x => x.TotalPrice);
            var totalSpendings = _db.Finances.Where(x => x.TotalPrice < 0).Sum(x => x.TotalPrice);
            var positiveSpendings = Math.Abs(totalSpendings);
            var totalTransactions = _db.Finances.Count();

            return Ok(new{
                TotalMoney = totalMoney,
                TotalProfit = totalProfit,
                TotalSpendings = positiveSpendings,
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

        [HttpGet("most-sold-product")]
        public IActionResult GetMostSoldProduct()
        {
            var product = _financeService.GetMostSoldProduct();

            if (product == null)
            {
                return Ok(new
                {
                    Message = "We have no sales, make a sale and it will display most sold product"
                });
            }
            return Ok(product);
        }
    }
}
