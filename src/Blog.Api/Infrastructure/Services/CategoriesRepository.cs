using Blog.Api.Entities;
using Blog.Api.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Infrastructure.Services
{
    public class CategoriesRepository : RepositoryBase<Categories, int>, ICategoriesRepository
    {
        public CategoriesRepository(DbContext dbContext) : base(dbContext)
        {

        }
        public async Task<IEnumerable<Categories>> GetAllCategoriesAsync()
        {
            return await DbContext.Set<Categories>()
                .Include(c => c.Articles)
                .ToListAsync();
        }
    }
}
