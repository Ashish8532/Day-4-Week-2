using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.DataAccess.Data;
using BlogSystem.DataAccess.Repository.IRepository;
using BlogSystem.Models.Models;

namespace BlogSystem.DataAccess.Repository
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Blog> UpdateAsync(Blog blog)
        {
            _context.Blogs.Update(blog);
            return blog;
        }
    }
}
