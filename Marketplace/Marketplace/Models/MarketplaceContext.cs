using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Models
{
    public class MarketplaceContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Category { get; set; }
        public MarketplaceContext(DbContextOptions<MarketplaceContext> options) : base(options)
        {

        }
    }
}
