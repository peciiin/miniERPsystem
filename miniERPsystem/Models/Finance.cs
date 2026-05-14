using System;
using System.Collections.Generic;

namespace miniERPsystem.Models;

public partial class Finance
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public decimal Quantity { get; set; }

    public decimal PricePerItem { get; set; }

    public decimal TotalPrice { get; set; }

    public string Currency { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Note { get; set; }

    public DateTime? Created { get; set; }

    public virtual Storage Item { get; set; } = null!;
}
