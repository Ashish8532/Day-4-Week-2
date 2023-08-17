using BlogSystem.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .HasMany(c => c.Comments)
                .WithOne(c => c.Blog)
                .HasForeignKey(c => c.BlogId)
                .HasPrincipalKey(c => c.Id);
        }
    }
}
