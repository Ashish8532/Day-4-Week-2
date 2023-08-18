using BlogSystem.DataAccess.Data;
using BlogSystem.DataAccess.Repository.IRepository;
using BlogSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Blog = new BlogRepository(_context);
        }
        public IBlogRepository Blog { get; private set; }
        public ICommentRepository Comment { get; private set; }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
