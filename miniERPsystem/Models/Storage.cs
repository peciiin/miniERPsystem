using System;
using System.Collections.Generic;

namespace miniERPsystem.Models;

public partial class Storage
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal? Quantity { get; set; }

    public string? Units { get; set; }

    public bool? IsFinal { get; set; }

    public virtual ICollection<Recipe> RecipeMaterials { get; set; } = new List<Recipe>();

    public virtual ICollection<Recipe> RecipeProducts { get; set; } = new List<Recipe>();
}
