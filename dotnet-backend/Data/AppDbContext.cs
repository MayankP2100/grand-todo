using dotnet_backend.Modules.Todos.Entities;
using Microsoft.EntityFrameworkCore;

namespace dotnet_backend.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<TodoItem> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global query filter to exclude soft-deleted items
            modelBuilder.Entity<TodoItem>().HasQueryFilter(t => t.DeletedAt == null);
        }
    }
}
