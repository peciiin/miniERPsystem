using Microsoft.EntityFrameworkCore;
using miniERPsystem.Models;
namespace miniERPsystem.Services
{
    public class FinanceService
    {
        private readonly MiniErpsystemContext _database;
        public FinanceService(MiniErpsystemContext db)
        {
            _database = db;
        }

        public async Task FinanceLogTransactionAsync(int itemId, decimal quantity, decimal pricePerItem, string type, string note = "")
        {
            bool isBuy = type == "PURCHASE";
            var multiply = isBuy ? -1 : 1;

            var log = new Finance
            {
                ItemId = itemId,
                Quantity = quantity,
                PricePerItem = pricePerItem,
                TotalPrice = pricePerItem * quantity * multiply,
                Currency = "CZK",
                Type = type,
                Note = note,
                Created = DateTime.Now
            };

            _database.Finances.Add(log);
            await _database.SaveChangesAsync();
        }

        public async Task<MostSoldProduct?> GetMostSoldProductAsync()
        {
            return await _database.Finances.Where(x => x.Type == "SALE").GroupBy(y => y.ItemId).Select(z => new MostSoldProduct
            {
                ProductId = z.Key,
                TotalSold = z.Sum(x => x.Quantity),
                TotalEarnings = z.Sum(x => x.TotalPrice)
            }).OrderByDescending(p => p.TotalEarnings).FirstOrDefaultAsync();


        }
    }
}
