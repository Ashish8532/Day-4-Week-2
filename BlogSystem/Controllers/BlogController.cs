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
using System.Reflection.PortableExecutable;

namespace BlogSystem.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BlogController(IUnitOfWork unitOfWork, IBlogRepository blogRepository)
        {
            _unitOfWork = unitOfWork;
            _blogRepository = blogRepository;
        }

        //Response caching reduces the number of requests a client or proxy makes to a web server.
        //Response caching also reduces the amount of work the web server performs to generate a
        //response.Response caching is set in headers.

        [HttpGet]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client)] 
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
                await _blogRepository.UpdateAsync(blog);

                // Save changes to the database
                await _unitOfWork.SaveAsync();

                // Redirect to the index action after successful update
                TempData["success"] = "Blog Updated Successfully.";
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

            using var transaction = _unitOfWork.BeginTransactionAsync();
            try
            {
                // Add the entity in the context and mark it as modified
                await _unitOfWork.Blog.Add(blog);
                // Save changes to the database
                await _unitOfWork.SaveAsync();

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                await _unitOfWork.CommitTransactionAsync();

                TempData["success"] = "Blog Created Successfully.";
                // Redirect to the index action after successful update
                return RedirectToAction("Index");

            }
            catch(Exception  ex)
            {
                // Handle exception
                await _unitOfWork.RollbackTransactionAsync();

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
