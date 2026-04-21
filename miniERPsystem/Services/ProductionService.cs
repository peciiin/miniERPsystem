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

        public ProductionResPattern craftItem(int idItemToCraft, decimal quantityToCraft) {
            
            var recipeItems = _databaseGate.Recipes.Where(x => x.ProductId == idItemToCraft).ToList();
            
            foreach (var recipe in recipeItems) {
                var inStorage = _databaseGate.Storages.Where(z => z.ItemId == recipe.MaterialId).FirstOrDefault();
                if (inStorage == null)
                {
                    return new ProductionResPattern { isSuccessed = false, message = "Material not found in storage" };
                }
                decimal totalNeed = (recipe.NeededMaterial ?? 0) * quantityToCraft;
                if (inStorage.Quantity < totalNeed)
                {
                    return new ProductionResPattern { isSuccessed = false, message = "Not enough " + inStorage.ItemName + " need more: " + (totalNeed - inStorage.Quantity) };
                };

                inStorage.Quantity -= totalNeed;
            }

            var finalProduct = _databaseGate.Storages.Where(z => z.ItemId == idItemToCraft).FirstOrDefault();
            if (finalProduct == null)
            {
                return new ProductionResPattern { isSuccessed = false, message = "Target product not registred in storage" };
            }
            finalProduct.Quantity += quantityToCraft;
            _databaseGate.SaveChanges();
            return new ProductionResPattern
            {
                isSuccessed = true,
                message = "Succefull, crafted: " + quantityToCraft + " of " + finalProduct.ItemName + " total in storage: " + finalProduct.Quantity
            };
            
        }
    }
}
