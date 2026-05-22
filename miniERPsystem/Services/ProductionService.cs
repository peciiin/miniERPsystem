using miniERPsystem.Models;
using Microsoft.EntityFrameworkCore;


namespace miniERPsystem.Services
{
    public class ProductionService
    {
        private readonly MiniErpsystemContext _databaseGate;

        public ProductionService(MiniErpsystemContext database)
        {
            _databaseGate = database;
        }

        public async Task<ResultPattern> CraftItemAsync(int idItemToCraft, decimal quantityToCraft) {

            // Validation before transaction
            if (quantityToCraft <= 0)
            {
                return new ResultPattern
                {
                    isSuccessed = false,
                    message = "Quantity must be more than 0."
                };
            }

            var productInfo = await _databaseGate.Storages.FirstOrDefaultAsync(y => y.ItemId == idItemToCraft);

            if (productInfo == null || productInfo.IsFinal == false)
            {
                return new ResultPattern
                {
                    isSuccessed = false,
                    message = "This item u cant craft. You must buy it. It is raw material"
                };
            }
            

            var recipeItems = await _databaseGate.Recipes.Where(x => x.ProductId == idItemToCraft).ToListAsync();

            if (recipeItems.Count == 0)
            {
                return new ResultPattern { isSuccessed = false, message = "No recipe found for this item." };
            }


            foreach (var recipe in recipeItems)
            {
                var inStorage = await _databaseGate.Storages.Where(z => z.ItemId == recipe.MaterialId).FirstOrDefaultAsync();
                if (inStorage == null)
                {
                    return new ResultPattern { isSuccessed = false, message = "Material not found in storage" };
                }
                decimal totalNeed = (recipe.NeededMaterial ?? 0) * quantityToCraft;
                if (inStorage.Quantity < totalNeed)
                {
                    return new ResultPattern { isSuccessed = false, message = "Not enough " + inStorage.ItemName + " need more: " + (totalNeed - inStorage.Quantity) };
                }
            }

            //Transaction

            using (var transaction = await _databaseGate.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var recipe in recipeItems)
                    {
                        var inStorage = await _databaseGate.Storages.FirstAsync(z => z.ItemId == recipe.MaterialId);
                        decimal totalNeed = (recipe.NeededMaterial ?? 0) * quantityToCraft;

                        inStorage.Quantity -= totalNeed;
                    }

                    var finalProduct = await _databaseGate.Storages.Where(z => z.ItemId == idItemToCraft).FirstOrDefaultAsync();
                    if (finalProduct == null)
                    {
                        return new ResultPattern { isSuccessed = false, message = "Target product not registred in storage" };
                    }

                    finalProduct.Quantity += quantityToCraft;

                    await _databaseGate.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ResultPattern
                    {
                        isSuccessed = true,
                        message = "Succefull, crafted: " + quantityToCraft + " of " + finalProduct.ItemName + " total in storage: " + finalProduct.Quantity
                    };


                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();

                    return new ResultPattern
                    {
                        isSuccessed = false,
                        message = "Transaction failed, went to state before."
                    };
                }
            }

                

            
            
        }
    }
}
