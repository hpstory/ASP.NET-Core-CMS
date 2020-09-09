using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Controllers.DtoParameters;
using WebApplication.Entities;
using WebApplication.Helpers;
using WebApplication.Infrastructure.PropertyMapping;
using WebApplication.Models.Comments;
using WebApplication.Utils.Extensions;

namespace WebApplication.Infrastructure.Services
{
    public class CommentsRepository : RepositoryBase<Comments, int>, ICommentsRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public CommentsRepository(CMSDbContext dbContext, IPropertyMappingService propertyMappingService) : base(dbContext)
        {
            _propertyMappingService = propertyMappingService;
        }
        public async Task<PagedList<Comments>> GetAllAsync (CommentResourceParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var queryExpression = DbContext.Set<Comments>()
                .Include(u => u.User).Where(a => a.Articles.ID == parameters.ArticleId);

            var mappingDictionary = _propertyMappingService.GetPropertyMapping<CommentsDto, Comments>();
            queryExpression = queryExpression.ApplySort(parameters.OrderBy, mappingDictionary);

            return await PagedList<Comments>.CreateAsync(queryExpression, parameters.PageNumber, parameters.PageSize);
        }
    }
}
