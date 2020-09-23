using Blog.Api.Entities;
using Blog.Api.Helpers;
using Blog.Api.Infrastructure.PropertyMapping;
using Blog.Api.Infrastructure.Repository;
using Blog.Api.Models.Articles;
using Blog.Api.Models.DtoParameters;
using Blog.Api.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Infrastructure.Services
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
            var queryExpression = DbContext.Set<Articles>()
                .Include(c => c.Category)
                // .Include(u => u.User)
                as IQueryable<Articles>;
            if (!string.IsNullOrWhiteSpace(parameters.CategoryName))
            {
                queryExpression = queryExpression.Where(c => c.Category.Name == parameters.CategoryName);
            }
            if (parameters.IsHot != null)
            {
                queryExpression = queryExpression.Where(c => c.IsHot == parameters.IsHot);
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

        public async Task<Articles> GetArticleAsync(int articleId)
        {
            return await DbContext.Set<Articles>()
                .Include(c => c.Category)
                // .Include(u => u.User)
                .FirstOrDefaultAsync(article => article.ID == articleId);
        }
    }
}
