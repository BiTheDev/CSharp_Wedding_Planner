using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Models{
    public class WeddingContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public WeddingContext(DbContextOptions<WeddingContext> options) : base(options) { }

        public DbSet<users> users{get;set;}

        public DbSet<weddings> weddings{get;set;}

        public DbSet<guests> guests{get;set;}
        
    }
}