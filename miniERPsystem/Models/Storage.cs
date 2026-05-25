using System;
using System.Collections.Generic;

namespace miniERPsystem.Models;

public partial class Storage
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal Quantity { get; set; }

    public string? Units { get; set; }

    public bool? IsFinal { get; set; }

    public decimal MinQuantity { get; set; }

    public decimal OptimalQuantity { get; set; }

    public decimal? PurchasePrice { get; set; }

    public virtual ICollection<Finance> Finances { get; set; } = new List<Finance>();

    public virtual ICollection<Recipe> RecipeMaterials { get; set; } = new List<Recipe>();

    public virtual ICollection<Recipe> RecipeProducts { get; set; } = new List<Recipe>();
}
