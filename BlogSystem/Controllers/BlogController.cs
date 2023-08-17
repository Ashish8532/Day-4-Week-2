using BlogSystem.DataAccess.Data;
using BlogSystem.Models.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
