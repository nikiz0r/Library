using System;
using System.Collections.Generic;
using System.Linq;
using Library.API2.Services;
using System.Linq.Dynamic.Core;

namespace Library.API2.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null) throw new ArgumentNullException("source");
            
            if (mappingDictionary == null) throw new ArgumentNullException("mappingDictionary");
            
            if (string.IsNullOrEmpty(orderBy)) return source;
            
            // the orderBy string is separated by ",", so we split it.
            var orderByAfterSplit = orderBy.Split(',');

            // apply each orderby clause in reverse order - otherwise, the IQueryable will be ordered in the wrong order
            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedOrderByClause = orderByClause.Trim();

                // if it ends with " desc", order desc, otherwise asc
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                // remove " asc" or " desc" from orderByClause, so we get the property name to look for in the mapping dictionary
                var indexOfFirstSpace = orderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                // find the matching property
                if (!mappingDictionary.ContainsKey(propertyName)) throw new ArgumentException($"Key mapping for {propertyName} is missing");

                // get the PropertyMappingValue
                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null) throw new ArgumentException("propertyMappingValue");

                // Run through the property names in reverse so the orderby clauses are applied in the correct order
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    // revert sort order if necessary
                    if (propertyMappingValue.Revert)
                        orderDescending = !orderDescending;

                    source = source.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));
                }
            }
            
            return source;
        }
    }
}