using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WebApplication.Entities;
using WebApplication.Models.Categories;

namespace WebApplication.Infrastructure.Services
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
