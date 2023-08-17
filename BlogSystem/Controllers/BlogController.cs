using BlogSystem.DataAccess.Data;
using BlogSystem.Models.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Policy;
using System.Xml;
using System.Diagnostics.Metrics;

namespace BlogSystem.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Load all blogs
            var blogs = _context.Blogs.ToList();
            return View(blogs);
        }

        public IActionResult LazyLoad(int blogId)
        {
            // Lazily loading Comments for a blog post
            var blogs = _context.Blogs.FirstOrDefault(b => b.Id == blogId);
            if (blogs != null)
            {
                return View(blogs);
            }
            return NotFound();

        }

        public IActionResult Edit(int id)
        {
            var blogs = _context.Blogs.FirstOrDefault(b => b.Id == id);
            return View(blogs);
        }

        [HttpPost]
        public IActionResult Edit(Blog blog)
        {
            try
            {
                // Check if the model state is valid (validations)
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Invalid Blog Details!";
                    return View(blog);
                }
                // Update the entity in the context and mark it as modified
                _context.Blogs.Update(blog).State = EntityState.Modified;
                // Save changes to the database
                _context.SaveChanges();
                // Redirect to the index action after successful update
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict

                // Get the database values of the conflicting entity
                var entry = ex.Entries.Single().GetDatabaseValues();

                // If the entity was deleted by another user
                if (entry == null)
                {
                    TempData["errro"] = "Unable to save changes. The entity you are trying " +
                        "to update was deleted by another user.";
                }
                else
                {
                    // The entity was modified by another user
                    var databaseValues = (Blog)entry.ToObject();
                    TempData["error"] = "The blog post was modified by another user.";
                    // Update the version (or timestamp) to handle the conflict
                    blog.Version = databaseValues.Version;
                }

                return View(blog);
            }
        }
    }
}
