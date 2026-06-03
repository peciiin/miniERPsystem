using Microsoft.EntityFrameworkCore;
using miniERPsystem.Models;

namespace miniERPsystem.Services
{
    public class ReportService
    {
        private readonly MiniErpsystemContext _database;

        public ReportService(MiniErpsystemContext database)
        {
            _database = database;
        }

        public async Task<DashboardReport> GetDashboardReportAsync()
        {
            var totalStorageValue = await _database.Storages
                .SumAsync(x => x.Quantity * (x.PurchasePrice ?? 0m));

            var lowItemsCount = await _database.Storages
                .CountAsync(x => x.Quantity < x.MinQuantity);

            var currentBalance = await _database.Finances
                .SumAsync(x => x.TotalPrice);

            return new DashboardReport(totalStorageValue, lowItemsCount, currentBalance);
        }
    }
    
    public record DashboardReport(decimal TotalStorageValue, int CriticalItemsCount, decimal CurrentBalance);
}