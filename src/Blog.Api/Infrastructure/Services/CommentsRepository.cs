using Blog.Api.Entities;
using Blog.Api.Helpers;
using Blog.Api.Infrastructure.PropertyMapping;
using Blog.Api.Infrastructure.Repository;
using Blog.Api.Models.Comments;
using Blog.Api.Models.DtoParameters;
using Blog.Api.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Infrastructure.Services
{
    public class CommentsRepository : RepositoryBase<Comments, int>, ICommentsRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public CommentsRepository(DbContext dbContext, IPropertyMappingService propertyMappingService) : base(dbContext)
        {
            _propertyMappingService = propertyMappingService;
        }
        public async Task<PagedList<Comments>> GetAllAsync(CommentResourceParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var queryExpression = DbContext.Set<Comments>()
                // .Include(u => u.User)
                .Where(a => a.Articles.ID == parameters.ArticleId);

            var mappingDictionary = _propertyMappingService.GetPropertyMapping<CommentsDto, Comments>();
            queryExpression = queryExpression.ApplySort(parameters.OrderBy, mappingDictionary);

            return await PagedList<Comments>.CreateAsync(queryExpression, parameters.PageNumber, parameters.PageSize);
        }
    }
}
