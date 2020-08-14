using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Entities;
using WebApplication.Helpers;
using WebApplication.Models.Articles;
using WebApplication.Models.Comments;

namespace WebApplication.Infrastructure.PropertyMapping
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _articlePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"ID", new PropertyMappingValue(new List<string>{"ID"}) },
                {"ArticleDate", new PropertyMappingValue(new List<string>{"PublishDate"})},
                {"CategoryID", new PropertyMappingValue(new List<string>{"CategoryID"})},
            };
        private readonly Dictionary<string, PropertyMappingValue> _commentPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"ID", new PropertyMappingValue(new List<string>{"ID"}) },
                {"Date", new PropertyMappingValue(new List<string>{"PublishTime"}) },
            };
        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();
        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<ArticlesDto, Articles>(_articlePropertyMapping));
            _propertyMappings.Add(new PropertyMapping<CommentsDto, Comments>(_commentPropertyMapping));
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
