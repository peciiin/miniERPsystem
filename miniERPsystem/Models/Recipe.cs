using System;
using System.Collections.Generic;

namespace miniERPsystem.Models;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public int? ProductId { get; set; }

    public int? MaterialId { get; set; }

    public decimal? NeededMaterial { get; set; }

    public virtual Storage? Material { get; set; }

    public virtual Storage? Product { get; set; }
}
