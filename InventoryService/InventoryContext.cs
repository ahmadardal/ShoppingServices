using Microsoft.EntityFrameworkCore;
using InventoryService.Model;

namespace InventoryService
{

    public class InventoryContext : DbContext
    {

        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

    }


}