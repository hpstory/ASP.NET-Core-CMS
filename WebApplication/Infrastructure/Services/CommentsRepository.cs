using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;

namespace WebApplication.Infrastructure.Services
{
    public class CommentsRepository : RepositoryBase<Comments, int>, ICommentsRepository
    {
        public CommentsRepository(CMSDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
