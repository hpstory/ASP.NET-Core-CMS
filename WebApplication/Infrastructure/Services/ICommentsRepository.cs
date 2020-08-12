using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;

namespace WebApplication.Infrastructure.Services
{
    public interface ICommentsRepository : IRepositoryBase<Comments>, IRepositoryBaseById<Comments, int>
    {
    }
}
