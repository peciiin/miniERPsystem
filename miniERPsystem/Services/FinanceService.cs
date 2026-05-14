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
    }
}
