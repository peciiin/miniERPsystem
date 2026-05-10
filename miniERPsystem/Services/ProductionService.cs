using miniERPsystem.Models;
namespace miniERPsystem.Services
{
    public class ProductionService
    {
        private readonly MiniErpsystemContext _databaseGate;

        public ProductionService(MiniErpsystemContext database)
        {
            _databaseGate = database;
        }

        public BuyCraftResPattern craftItem(int idItemToCraft, decimal quantityToCraft) {
            if (quantityToCraft <= 0)
            {
                return new BuyCraftResPattern
                {
                    isSuccessed = false,
                    message = "Quantity must be more than 0."
                };
            }

            var productInfo = _databaseGate.Storages.FirstOrDefault(y => y.ItemId == idItemToCraft);
            if (productInfo == null || productInfo.IsFinal == false)
            {
                return new BuyCraftResPattern
                {
                    isSuccessed = false,
                    message = "This item u cant craft. You must buy it. It is raw material"
                };
            }
            

            var recipeItems = _databaseGate.Recipes.Where(x => x.ProductId == idItemToCraft).ToList();

            if (recipeItems.Count == 0)
            {
                return new BuyCraftResPattern { isSuccessed = false, message = "No recipe found for this item." };
            }

            foreach (var recipe in recipeItems) {
                var inStorage = _databaseGate.Storages.Where(z => z.ItemId == recipe.MaterialId).FirstOrDefault();
                if (inStorage == null)
                {
                    return new BuyCraftResPattern { isSuccessed = false, message = "Material not found in storage" };
                }
                decimal totalNeed = (recipe.NeededMaterial ?? 0) * quantityToCraft;
                if (inStorage.Quantity < totalNeed)
                {
                    return new BuyCraftResPattern { isSuccessed = false, message = "Not enough " + inStorage.ItemName + " need more: " + (totalNeed - inStorage.Quantity) };
                };

                inStorage.Quantity -= totalNeed;
            }

            var finalProduct = _databaseGate.Storages.Where(z => z.ItemId == idItemToCraft).FirstOrDefault();
            if (finalProduct == null)
            {
                return new BuyCraftResPattern { isSuccessed = false, message = "Target product not registred in storage" };
            }
            finalProduct.Quantity += quantityToCraft;
            _databaseGate.SaveChanges();
            return new BuyCraftResPattern
            {
                isSuccessed = true,
                message = "Succefull, crafted: " + quantityToCraft + " of " + finalProduct.ItemName + " total in storage: " + finalProduct.Quantity
            };
            
        }
    }
}
