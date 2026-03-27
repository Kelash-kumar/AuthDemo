using Microsoft.EntityFrameworkCore;

namespace AuthDemo.Data
{
    public partial class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        }


    }

}
