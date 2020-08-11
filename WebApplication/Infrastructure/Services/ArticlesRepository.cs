using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Controllers.DtoParameters;
using WebApplication.Entities;
using WebApplication.Helpers;

namespace WebApplication.Infrastructure.Services
{
    public class ArticlesRepository : RepositoryBase<Articles, int>, IArticlesRepository
    {
        public ArticlesRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedList<Articles>> GetAllAsync(ArticleResourceParameters parameters)
        {
            var queryExpression = _dbContext.Set<Articles>() as IQueryable<Articles>;

            return await PagedList<Articles>.CreateAsync(queryExpression, parameters.PageNumber, parameters.PageSize);
        }
    }
}
