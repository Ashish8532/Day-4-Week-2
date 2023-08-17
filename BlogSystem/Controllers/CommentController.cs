using BlogSystem.DataAccess.Data;
using BlogSystem.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var comment = _context.Comments.Include(blog => blog.Blog).ToList();
            return View(comment);
        }

        
    }
}
