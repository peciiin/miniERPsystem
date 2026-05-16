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

        public void FinanceLogTransaction(int itemId, decimal quantity, decimal pricePerItem, string type, string note = "")
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
        }

        public MostSoldProduct? GetMostSoldProduct()
        {
            return _database.Finances.Where(x => x.Type == "SALE").GroupBy(y => y.ItemId).Select(z => new MostSoldProduct
            {
                ProductId = z.Key,
                TotalSold = z.Sum(x => x.Quantity),
                TotalEarnings = z.Sum(x => x.TotalPrice)
            }).OrderByDescending(p => p.TotalEarnings).FirstOrDefault();


        }
    }
}
