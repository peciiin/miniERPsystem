using miniERPsystem.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ResultPattern> SellItemAsync(int id, decimal quantity, decimal pricePerItem, string note)
        {
            // Sell Validation
            if (pricePerItem < 0)
            {
                return new ResultPattern()
                {
                    isSuccessed = false,
                    message = "Price cant be lower than 0 when u selling"
                };
            }
            
            if (quantity < 0)
            {
                return new ResultPattern()
                {
                    isSuccessed = false,
                    message = "Quantity must be grater than 0"
                };
            }
            var item = await _databaseGate.Storages.FirstOrDefaultAsync(x => x.ItemId == id);
            if (item == null)
            {
                return new ResultPattern { isSuccessed = false, message = "Item does not exsits in storage" };
            }

            if (item.IsFinal == false)
            {
                return new ResultPattern { isSuccessed = false, message = "This item u cant sell" };
            }

            if (item.Quantity < quantity)
            {
                return new ResultPattern
                {
                    isSuccessed = false,
                    message = "You want to sell more than u have in storage, maximum quaintity is: " + item.Quantity.ToString()
                };
            }
            

            // Finance transaction
            using(var transaction = await _databaseGate.Database.BeginTransactionAsync())
            {
                try
                {
                    item.Quantity -= quantity;

                    await _financeService.FinanceLogTransactionAsync(id, quantity, pricePerItem, "SALE", note);
                    await _databaseGate.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ResultPattern
                    {
                        isSuccessed = true,
                        message = "Succefully sold " + quantity + " " + item.Units + " of " + item.ItemName + " from storage for total price of: " + pricePerItem * quantity + " CZK."
                    };
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ResultPattern
                    {
                        isSuccessed = false,
                        message = "Transaction failed, went to state before selling"
                    };
                }
                
            }
            
        }
    }
}
