using miniERPsystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace miniERPsystem.Services
{
    public class PurchaseService
    {
        private readonly MiniErpsystemContext _databaseGate;
        private readonly FinanceService _financeService;
        public PurchaseService(MiniErpsystemContext databaseGate, FinanceService financeService)
        {
            _databaseGate = databaseGate;
            _financeService = financeService;
        }

        public async Task<ResultPattern> BuyItem(int id, decimal quantity, decimal pricePerItem, string note)
        {

            // Validation before transaction
            if (quantity < 0)
            {
                return new ResultPattern
                {
                    isSuccessed = false,
                    message = "Buy more than 0"
                };
            }

            var item = await _databaseGate.Storages.FirstOrDefaultAsync(x => x.ItemId == id);

            if (item == null)
            {
                return new ResultPattern { isSuccessed = false, message = "Item does not exsits in storage" };
            }

            if (item.IsFinal == true)
            {
                return new ResultPattern { isSuccessed = false, message = "This item u must craft, not buy" };
            }
            

            // Transaction
            using(var transaction = await _databaseGate.Database.BeginTransactionAsync())
            {
                try
                {
                    item.Quantity += quantity;

                    await _financeService.FinanceLogTransactionAsync(id, quantity, pricePerItem, "PURCHASE", note);
                    await _databaseGate.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ResultPattern
                    {
                        isSuccessed = true,
                        message = "Succefully bought " + quantity + " " + item.Units + " of " + item.ItemName + " to storage for total price of: " + quantity * pricePerItem + " CZK."
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ResultPattern
                    {
                        isSuccessed = false,
                        message = "Transaction failed, returning to previous state."
                    };
                }
            }
            
        }
    }
}
