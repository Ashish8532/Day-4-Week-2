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
using BlogSystem.DataAccess.Repository.IRepository;

namespace BlogSystem.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public BlogController(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async  Task<IActionResult> Index()
        {
            // Load all blogs
            var blogs = await _unitOfWork.Blog.GetAll();
            return View(blogs);
        }

        public async Task<IActionResult> LazyLoad(int blogId)
        {
            // Lazily loading Comments for a blog post
            var blogs = await _unitOfWork.Blog.GetFirstOrDefault(b => b.Id == blogId);
            if (blogs != null)
            {
                return View(blogs);
            }
            return NotFound();

        }

        public async Task<IActionResult> Edit(int id)
        {
            var blogs = await _unitOfWork.Blog.GetById(id);
            return View(blogs);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Blog blog)
        {
            // Check if the model state is valid (validations)
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid Blog Details!";
                return View(blog);
            }

            try
            {
                // Update the entity in the context and mark it as modified
                await _unitOfWork.Blog.UpdateAsync(blog);
                
                // Redirect to the index action after successful update
                TempData["success"] = "Blog Updated Successfully.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Message from HandleConcurrencyConflict method
                TempData["error"] = ex.Message;
                return View(blog);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid Blog Details!";
                return View(blog);
            }

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Add the entity in the context and mark it as modified
                await _unitOfWork.Blog.Add(blog);
                // Save changes to the database
                await _unitOfWork.Save();

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                transaction.Commit();

                TempData["success"] = "Blog Created Successfully.";
                // Redirect to the index action after successful update
                return RedirectToAction("Index");

            }
            catch(Exception  ex)
            {
                // Handle exception
                transaction.Rollback();

                TempData["error"] = $"{ex.Message}!";
                return View(blog);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Blog.GetById(id);
            if (product != null)
            {
                await _unitOfWork.Blog.Delete(product);
                return Json(new { success = true });
            }
            else
            {
                TempData["error"] = "Product not found.";
                return View(product);
            }
        }
    }
}
