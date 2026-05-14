using miniERPsystem.Models;

namespace miniERPsystem.Services
{
    public class SellService
    {
        private readonly MiniErpsystemContext _databaseGate;
        private readonly FinanceService _financeService;
        public SellService(MiniErpsystemContext databaseGate, FinanceService financeService)
        {
            _databaseGate = databaseGate;
            _financeService = financeService;
        }

        public BuyCraftResPattern SellItem(int id, decimal quantity, decimal pricePerItem, string note)
        {
            //Sell checking
            if (quantity < 0)
            {
                return new BuyCraftResPattern()
                {
                    isSuccessed = false,
                    message = "Quantity must be grater than 0"
                };
            }
            var item = _databaseGate.Storages.FirstOrDefault(x => x.ItemId == id);
            if (item == null)
            {
                return new BuyCraftResPattern { isSuccessed = false, message = "Item does not exsits in storage" };
            }

            if (item.IsFinal == false)
            {
                return new BuyCraftResPattern { isSuccessed = false, message = "This item u cant sell" };
            }

            if (item.Quantity < quantity)
            {
                return new BuyCraftResPattern
                {
                    isSuccessed = false,
                    message = "You want to sell more than u have in storage, maximum quaintity is: " + item.Quantity.ToString()
                };
            }
            item.Quantity -= quantity;

            // Finance logging

            _financeService.FinanceLogTransaction(id, quantity, pricePerItem, "SALE", note);

            _databaseGate.SaveChanges();

            return new BuyCraftResPattern
            {
                isSuccessed = true,
                message = "Succefully sold " + quantity + " " + item.Units + " of " + item.ItemName + " from storage for total price of: " + pricePerItem * quantity + " CZK."
            };
        }
    }
}
