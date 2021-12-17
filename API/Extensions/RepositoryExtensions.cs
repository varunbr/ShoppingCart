using API.DTOs;
using API.Entities;
using API.Helpers;
using System.Collections.Generic;

namespace API.Extensions
{
    public static class RepositoryExtensions
    {
        public static List<PropertyValueDto> GetPropertyValue(this IEnumerable<PropertyValue> propertyValues)
        {
            var result = new List<PropertyValueDto>();

            foreach (var propertyValue in propertyValues)
            {
                result.Add(new PropertyValueDto
                {
                    Name = propertyValue.Property.Name,
                    Value = propertyValue.Property.Type == Type.Integer
                        ? $"{propertyValue.IntegerValue} {propertyValue.Property.Unit}".Trim()
                        : propertyValue.StringValue
                });
            }

            return result;
        }
    }
}
