using BlogSystem.DataAccess.Data;
using BlogSystem.DataAccess.Repository.IRepository;
using BlogSystem.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DataAccess.Repository
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(Blog blog)
        {
            try
            {
                _context.Blogs.Update(blog).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                HandleConcurrencyConflict(ex, blog);
            }
        }

        private void HandleConcurrencyConflict(DbUpdateConcurrencyException ex, Blog blog)
        {
            // Handle concurrency conflict
            // Get the database values of the conflicting entity
            var entry = ex.Entries.Single();
            var databaseValues = entry.GetDatabaseValues();

            // If the entity was deleted by another user
            if (databaseValues == null)
            {
                throw new DbUpdateConcurrencyException("Unable to save changes. The entity you are trying " +
                    "to update was deleted by another user.");
            }
            // The entity was modified by another user
            var databaseEntity = (Blog)databaseValues.ToObject();

            // Update the version (or timestamp) to handle the conflict
            blog.Version = databaseEntity.Version;
            throw new DbUpdateConcurrencyException("The blog post was modified by another user.");
        }



    }
}
