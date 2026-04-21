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

        public string craftItem(int idItemToCraft, int quantityToCraft) {
            // only ID of material we need returning
            var recipeItems = _databaseGate.Recipes.Where(x => x.ProductId == idItemToCraft).ToList();
            
            foreach (var recipe in recipeItems) {
                var inStorage = _databaseGate.Storages.Where(z => z.ItemId == recipe.MaterialId).FirstOrDefault();
                if (inStorage == null)
                {
                    return "¨Material " + recipe.MaterialId + " neni vubec v systemu";
                }
                decimal totalNeed = (recipe.NeededMaterial ?? 0) * quantityToCraft;
                if (inStorage.Quantity < totalNeed)
                {
                    return "Malo " + inStorage.ItemName + " chybí: " + (totalNeed - inStorage.Quantity);
                }

                inStorage.Quantity -= totalNeed;
            }

            var finalProduct = _databaseGate.Storages.Where(z => z.ItemId == idItemToCraft).FirstOrDefault();
            if (finalProduct == null)
            {
                return "Produkt s ID" + idItemToCraft + " neexistuje v storage";
            }
            finalProduct.Quantity += quantityToCraft;
            _databaseGate.SaveChanges();
            return "Uspesne provedeno, vyrobeno: " + quantityToCraft + " " + finalProduct.ItemName + ". Celkem na sklede: " + finalProduct.Quantity;
        }
    }
}
