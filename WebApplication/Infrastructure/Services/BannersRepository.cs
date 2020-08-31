using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using WebApplication.Entities;
using WebApplication.Infrastructure.PropertyMapping;
using WebApplication.Models;
using WebApplication.Utils.Extensions;

namespace WebApplication.Infrastructure.Services
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
