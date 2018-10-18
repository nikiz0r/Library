using System.Collections.Generic;

namespace Library.API2.Services
{
    public interface IPropertyMappingService
    {
         Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}