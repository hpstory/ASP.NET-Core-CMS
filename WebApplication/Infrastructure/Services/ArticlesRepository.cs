using Microsoft.EntityFrameworkCore;
using System;
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
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            var queryExpression = _dbContext.Set<Articles>() as IQueryable<Articles>;
            if (!(parameters.CategoryId == null))
            {
                queryExpression = queryExpression.Where(c => c.CategoryID == parameters.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                queryExpression = queryExpression.Where(
                    t => t.Title.Contains(parameters.SearchQuery) || t.Content.Contains(parameters.SearchQuery));
            }

            queryExpression = queryExpression.ApplySort(parameters.OrderBy, mappingDictionary);

            return await PagedList<Articles>.CreateAsync(queryExpression, parameters.PageNumber, parameters.PageSize);
        }
    }
}
