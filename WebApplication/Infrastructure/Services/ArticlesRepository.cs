using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Controllers.DtoParameters;
using WebApplication.Entities;
using WebApplication.Helpers;
using WebApplication.Infrastructure.PropertyMapping;
using WebApplication.Models.Articles;

namespace WebApplication.Infrastructure.Services
{
    public class ArticlesRepository : RepositoryBase<Articles, int>, IArticlesRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public ArticlesRepository(DbContext dbContext, IPropertyMappingService propertyMappingService) : base(dbContext)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<PagedList<Articles>> GetAllAsync(ArticleResourceParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            var queryExpression = DbContext.Set<Articles>() as IQueryable<Articles>;
            if (!(parameters.CategoryId == null))
            {
                queryExpression = queryExpression.Where(c => c.CategoryID == parameters.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                queryExpression = queryExpression.Where(
                    t => t.Title.Contains(parameters.SearchQuery) || t.Content.Contains(parameters.SearchQuery));
            }

            var mappingDictionary = _propertyMappingService.GetPropertyMapping<ArticlesDto, Articles>();
            queryExpression = queryExpression.ApplySort(parameters.OrderBy, mappingDictionary);

            return await PagedList<Articles>.CreateAsync(queryExpression, parameters.PageNumber, parameters.PageSize);
        }
    }
}
