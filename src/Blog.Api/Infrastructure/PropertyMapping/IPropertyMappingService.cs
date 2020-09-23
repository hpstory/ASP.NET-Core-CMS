using Blog.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Infrastructure.PropertyMapping
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}
