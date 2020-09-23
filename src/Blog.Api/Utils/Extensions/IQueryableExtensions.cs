using Blog.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Blog.Api.Utils.Extensions
{
    public static class IQueryableExtensions
    {
        const string OrderClauseDesc = "desc";
        const string OrderClauseAsc = "asc";
        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> source,
            string orderBy,
            Dictionary<string, PropertyMappingValue> mapping
            ) where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (mapping == null)
            {
                throw new ArgumentNullException(nameof(mapping));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }
            List<string> orderByParts = new List<string>();
            var allQueryParts = orderBy.Split(",");
            foreach (var queryPart in allQueryParts)
            {
                var trimOrderBy = queryPart.Trim();
                var isDescending = trimOrderBy.EndsWith(OrderClauseDesc);
                var propertyName = isDescending ? queryPart.Substring(0, queryPart.Length - OrderClauseDesc.Length - 1) : queryPart.Trim();
                if (!mapping.ContainsKey(propertyName))
                {
                    throw new ArgumentNullException($"没有找到Key为{propertyName}的映射");
                }
                var propertyMappingValue = mapping[propertyName];
                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException(nameof(propertyMappingValue));
                }
                foreach (var targetProperties in propertyMappingValue.TargetProperties)
                {
                    if (propertyMappingValue.Revert)
                    {
                        isDescending = !isDescending;
                    }
                    orderByParts.Add($"{targetProperties} " + (isDescending ? OrderClauseDesc : OrderClauseAsc));
                }
            }
            source = source.OrderBy(string.Join(',', orderByParts));
            return source;
        }
    }
}
