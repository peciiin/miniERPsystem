namespace miniERPsystem.Models
{
    public class AutomaticOrderCheckResult
    {
        public class ReorderCheckResult
        {
            public bool IsRequired { get; set; }
            public decimal QuantityToBuy { get; set; }
        }
    }
}
