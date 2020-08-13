using System.Collections.Generic;
using WebApplication.Helpers;

namespace WebApplication.Infrastructure.PropertyMapping
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}
