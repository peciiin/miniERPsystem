using miniERPsystem.Models;

namespace miniERPsystem.Services
{
    public class PurchaseService
    {
        private readonly MiniErpsystemContext _databaseGate;
        public PurchaseService(MiniErpsystemContext databaseGate)
        {
            _databaseGate = databaseGate;
        }

        public BuyCraftResPattern BuyItem(int id, decimal quantity)
        {
            if (quantity < 0)
            {
                return new BuyCraftResPattern
                {
                    isSuccessed = false,
                    message = "Buy more than 0"
                };
            }

            var item = _databaseGate.Storages.FirstOrDefault(x => x.ItemId == id);

            if (item == null)
            {
                return new BuyCraftResPattern { isSuccessed = false, message = "Item does not exsits in storage" };
            }

            if (item.IsFinal == true)
            {
                return new BuyCraftResPattern { isSuccessed = false, message = "This item u must craft, not buy" };
            }
            item.Quantity += quantity;
            _databaseGate.SaveChanges();
            return new BuyCraftResPattern
            {
                isSuccessed = true,
                message = "Succefully bought " + quantity + " " + item.Units + " of " + item.ItemName + " to storage."
            };
        }
    }
}
