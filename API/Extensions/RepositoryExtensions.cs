using API.DTOs;
using API.Entities;
using API.Helpers;
using System.Collections.Generic;
using System.Linq;

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
                        ? propertyValue.IntegerValue == -1 ? "NA" : $"{propertyValue.IntegerValue} {propertyValue.Property.Unit}".Trim()
                        : propertyValue.StringValue
                });
            }

            return result;
        }

        public static ProductModelDto GetProductModel(this List<ProductDetailDto> products)
        {
            var productModel = new ProductModelDto();

            var properties = new List<PropertyValueDto>();

            foreach (var product in products)
            {
                productModel.Products.Add(product.Id, product);
                properties.AddRange(product.Properties);
            }

            foreach (var property in properties.Select(p => p.Name).Distinct())
            {
                var distinctValues = properties
                    .Where(p => p.Name == property)
                    .Select(p => p.Value)
                    .Distinct().ToList();

                if (distinctValues.Count <= 1)
                    continue;

                productModel.Variants.Add(new PropertyVariant
                {
                    Name = property,
                    Values = distinctValues
                });
            }
            return productModel;
        }
    }
}
