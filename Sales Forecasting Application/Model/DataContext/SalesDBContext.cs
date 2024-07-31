using Microsoft.EntityFrameworkCore;

namespace Sales_Forecasting_Application.Model.DataContext
{
    public class SalesDBContext:DbContext
    {
        public SalesDBContext(DbContextOptions<SalesDBContext> options):base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<OrderReturns> OrdersReturns { get; set; }
        public DbSet<Orders> Orders { get; set; }
    }
}
