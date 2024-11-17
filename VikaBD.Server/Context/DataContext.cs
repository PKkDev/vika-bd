using Microsoft.EntityFrameworkCore;
using VikaBD.Server.Model;

namespace VikaBD.Server.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Guest> Guest { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
        }
    }
}
