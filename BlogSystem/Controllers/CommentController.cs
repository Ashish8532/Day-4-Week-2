using BlogSystem.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Controllers
{
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var comment = await _unitOfWork.Comment.GetAll();
            return View(comment);
        }


    }
}
