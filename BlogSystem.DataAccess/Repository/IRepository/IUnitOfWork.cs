using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IBlogRepository Blog { get; }
        ICommentRepository Comment { get; }

        Task SaveAsync();

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
