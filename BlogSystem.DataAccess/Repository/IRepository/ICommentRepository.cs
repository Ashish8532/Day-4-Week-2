using BlogSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DataAccess.Repository.IRepository
{
    public interface ICommentRepository : IRepository<Comment>
    {
    }
}
