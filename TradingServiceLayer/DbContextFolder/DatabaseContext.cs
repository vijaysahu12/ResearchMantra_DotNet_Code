using Microsoft.EntityFrameworkCore;
using TradingServiceLayer.Entity;

namespace TradingServiceLayer.DbContextFolder
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<LiveStockEntity> LiveStock { get; set; }
    }
}
