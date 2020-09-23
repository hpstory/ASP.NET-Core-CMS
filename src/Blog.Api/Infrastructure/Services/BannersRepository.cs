using Blog.Api.Entities;
using Blog.Api.Infrastructure.PropertyMapping;
using Blog.Api.Infrastructure.Repository;
using Blog.Api.Models.Banners;
using Blog.Api.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Infrastructure.Services
{
    public class BannersRepository : RepositoryBase<Banners, int>, IBannersRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public BannersRepository(DbContext dbContext, IPropertyMappingService propertyMappingService) : base(dbContext)
        {
            _propertyMappingService = propertyMappingService;
        }
        public new async Task<IEnumerable<Banners>> GetAllAsync()
        {
            var queryExpression = DbContext.Set<Banners>().OrderBy("Position") as IQueryable<Banners>;
            var mappingDictionary = _propertyMappingService.GetPropertyMapping<BannersDto, Banners>();
            queryExpression = queryExpression.ApplySort("Position", mappingDictionary);
            return await queryExpression.ToListAsync();
        }
    }
}
