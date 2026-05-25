using Microsoft.EntityFrameworkCore;
using miniERPsystem.Models;
using System.Linq;
using static miniERPsystem.Models.AutomaticOrderCheckResult;

namespace miniERPsystem.Services
{
    public class AutomaticOrderService
    {
        private readonly MiniErpsystemContext _database;
        public readonly FinanceService _financeService;
        
        public AutomaticOrderService(MiniErpsystemContext database, FinanceService financeService) 
        {
            _database = database;
            _financeService = financeService;
        }

        public async Task<AutomaticOrderCheckResult> CheckStorageItemQuantity(int itemId)
        {
            var orderResult = await _database.Storages
                .Where(x => x.ItemId == itemId)
                .Select(x => new AutomaticOrderCheckResult(
                    x.Quantity < x.MinQuantity,
                    x.Quantity < x.MinQuantity ? (x.OptimalQuantity - x.Quantity) : 0m
                ))
                .FirstOrDefaultAsync();

            if (orderResult == null)
            {
                return new AutomaticOrderCheckResult(false, 0m);
            }

            return orderResult;
        }

        public async Task<string> DoAutomaticOrderAsync(int itemId, decimal quantityToBuy)
        {
            var item = await _database.Storages.FirstOrDefaultAsync(x => x.ItemId == itemId);
            if (item == null) return string.Empty;

            decimal purchasePrice = item.PurchasePrice ?? 0m;
            decimal totalCost = quantityToBuy * purchasePrice;




            var currentBalance = await _database.Finances.SumAsync(x => x.TotalPrice);
            if (currentBalance < totalCost)
            {
                return $" [WARNING: Low stock on {item.ItemName}. Automatic order failed: Insufficient funds. Available: {currentBalance} CZK, Required: {totalCost} CZK]";
            }

            using (var transaction = await _database.Database.BeginTransactionAsync())
            {
                try
                {
                    item.Quantity += quantityToBuy;

                    await _financeService.FinanceLogTransactionAsync(
                        item.ItemId,
                        quantityToBuy,
                        purchasePrice,
                        "PURCHASE",
                        "Automatic order: Low stock trigger"
                    ); 
                    await _database.SaveChangesAsync();
                    await transaction.CommitAsync();



                    return item.ItemName + " is bellow minimum recommended quantity, automatically ordered " + quantityToBuy + " for price of " + totalCost + "CZK.";
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    
                    return "Order failed: " + ex;
                }
            }
        }


    }

    public record AutomaticOrderCheckResult(bool IsRequired, decimal QuantityToBuy);
}
