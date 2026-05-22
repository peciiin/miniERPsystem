using Microsoft.AspNetCore.Mvc;
using miniERPsystem.Models;
using miniERPsystem.Services;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet("balance")] // Gets total return of money, Total profit from sells, Total spendings on material and total transactions made
        public async Task<IActionResult> GetFinanceBalance()
        {
            var totalMoney = await _db.Finances.SumAsync(x => x.TotalPrice);
            var totalProfit = await _db.Finances.Where(x => x.TotalPrice > 0).SumAsync(x => x.TotalPrice);
            var totalSpendings = await _db.Finances.Where(x => x.TotalPrice < 0).SumAsync(x => x.TotalPrice);
            var positiveSpendings = Math.Abs(totalSpendings);
            var totalTransactions = await _db.Finances.CountAsync();

            return Ok(new{
                TotalMoney = totalMoney,
                TotalProfit = totalProfit,
                TotalSpendings = positiveSpendings,
                Currency = "CZK",
                TotalTransactions = totalTransactions
            });
        }

        [HttpGet("history")] // Gets history of transactions
        public async Task<IActionResult> GetHistory()
        {
            var history = await _db.Finances
                .OrderByDescending(f => f.Created)
                .ToListAsync();

            return Ok(history);
        }

        [HttpGet("most-sold-product")]
        public async Task<IActionResult> GetMostSoldProduct()
        {
            var product = await _financeService.GetMostSoldProductAsync();

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
