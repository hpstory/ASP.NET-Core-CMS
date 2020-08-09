using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;

namespace WebApplication.Infrastructure.Services
{
    public class ArticlesRepository : RepositoryBase<Articles, int>, IArticlesRepository
    {
        public ArticlesRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
