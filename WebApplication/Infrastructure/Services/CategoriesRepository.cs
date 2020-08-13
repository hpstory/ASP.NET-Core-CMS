using Microsoft.EntityFrameworkCore;
using WebApplication.Entities;

namespace WebApplication.Infrastructure.Services
{
    public class CategoriesRepository : RepositoryBase<Categories, int>, ICategoriesRepository
    {
        public CategoriesRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
