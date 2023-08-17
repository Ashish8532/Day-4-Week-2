using BlogSystem.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .HasMany(c => c.Comments)
                .WithOne(c => c.Blog)
                .HasForeignKey(c => c.BlogId)
                .HasPrincipalKey(c => c.Id);

            modelBuilder.Entity<Blog>().HasData(
                new Blog { Id = 1, Title = "Blog 1", Content = "This is Blog 1." },
                new Blog { Id = 2, Title = "Blog 2", Content = "This is Blog 2." },
                new Blog { Id = 3, Title = "Blog 3", Content = "This is Blog 3." }
                );

            modelBuilder.Entity<Comment>().HasData(
                new Comment { Id = 1, Content = "This is Comment 1.", BlogId = 1 },
                new Comment { Id = 2, Content = "This is Comment 2.", BlogId = 1 },
                new Comment { Id = 3, Content = "This is Comment 3.", BlogId = 1 },  
                
                new Comment { Id = 4, Content = "This is Comment 4.", BlogId = 2 },     
                new Comment { Id = 5, Content = "This is Comment 5.", BlogId = 2 }, 
                
                new Comment { Id = 6, Content = "This is Comment 6.", BlogId = 3 },     
                new Comment { Id = 7, Content = "This is Comment 7.", BlogId = 3 }    
                );

            modelBuilder.Entity<Blog>()
                       .Property(p => p.Version).IsConcurrencyToken();
        }
    }
}
