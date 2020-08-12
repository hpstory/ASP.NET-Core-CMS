using AutoMapper.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;
using WebApplication.Helpers;
using WebApplication.Models.Articles;

namespace WebApplication.Infrastructure.PropertyMapping
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _articlePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"ID", new PropertyMappingValue(new List<string>{"ID"}) },
                {"Title", new PropertyMappingValue(new List<string>{"Title"}) },
                {"Author", new PropertyMappingValue(new List<string>{"Author"}) },
                {"Cover", new PropertyMappingValue(new List<string>{ "Cover"})},
                {"ArticleDate", new PropertyMappingValue(new List<string>{"PublishDate"})},
                {"Content", new PropertyMappingValue(new List<string>{"Content"})},
                {"CategoryID", new PropertyMappingValue(new List<string>{"CategoryID"})},
                {"PublisherID", new PropertyMappingValue(new List<string>{"PublisherID"})},
            };

        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();
        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<ArticlesDto, Articles>(_articlePropertyMapping));
        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            var propertyMappings = matchingMapping.ToList();
            if (propertyMappings.Count == 1)
            {
                return propertyMappings.First().MappingDictionary;
            }

            throw new Exception($"无法找到唯一的映射关系：{typeof(TSource)}, {typeof(TDestination)}");
        }
    }
}
