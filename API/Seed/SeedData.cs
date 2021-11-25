using API.Data;
using API.Entities;
using API.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Seed
{
    public class SeedData
    {
        public static async Task SeedRoles(RoleManager<IdentityRole<int>> roleManager)
        {
            if (await roleManager.Roles.AnyAsync()) return;

            foreach (var role in Enum.GetNames<Role>())
            {
                await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
            }
        }

        public static async Task SeedLocation(DataContext context, IMapper mapper)
        {
            if (await context.Locations.AnyAsync()) return;

            var data = await File.ReadAllTextAsync("Seed/LocationSeed.json");
            var country = JsonSerializer.Deserialize<Country>(data);
            var location = mapper.Map<Location>(country);

            await context.Locations.AddRangeAsync(location);
            await context.SaveChangesAsync();
        }

        public static async Task SeedCategory(DataContext context, IMapper mapper)
        {
            if (await context.Categories.AnyAsync()) return;

            var data = await File.ReadAllTextAsync("Seed/CategorySeed.json");
            var productCategories = JsonSerializer.Deserialize<List<ProductCategory>>(data);
            var categories = new List<Category>();

            if (productCategories != null)
                categories.AddRange(productCategories.Select(mapper.Map<Category>));

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        public static async Task SeedProduct(DataContext context, IMapper mapper)
        {
            if (await context.Products.AnyAsync()) return;

            var data = await File.ReadAllTextAsync("Seed/ProductSeed.json");
            var productSeeds = JsonSerializer.Deserialize<List<ProductSeed>>(data);

            if (productSeeds != null)
                foreach (var productSeed in productSeeds)
                {
                    var product = mapper.Map<Product>(productSeed);
                    var category = await context.Categories
                        .Include(c=>c.Properties)
                        .SingleAsync(c => c.Name == productSeed.Category);
                    product.Category = category;
                    product.Properties = new List<PropertyValue>();
                    product.ProductViews = new List<ProductView>();

                    foreach (var propertySeed in productSeed.Properties)
                    {
                        var property = category.Properties.Single(p => p.Name == propertySeed.Name);
                        product.Properties.Add(new PropertyValue
                        {
                            Property = property,
                            Value = propertySeed.Value
                        });
                    }

                    for (var i = 0; i < productSeed.Urls.Length; i++)
                    {
                        var productView = new ProductView
                        {
                            IsMain = i==0,
                            Photo = new Photo
                            {
                                Url = productSeed.Urls[i]
                            }
                        };
                        product.ProductViews.Add(productView);
                    }
                    await context.Products.AddAsync(product);
                    await context.SaveChangesAsync();
                }
        }
    }

    public class Country
    {
        public string Name { get; set; }
        public List<State> States { get; set; }
    }

    public class State
    {
        public string Name { get; set; }
        public List<City> Cities { get; set; }
    }

    public class City
    {
        public string Name { get; set; }
        public List<string> Areas { get; set; }
    }

    public class ProductCategory
    {
        public string Category { get; set; }
        public List<Property> Properties { get; set; }
        public List<ProductCategory> SubCategories { get; set; }
    }

    public class Property
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Values { get; set; }
        public string Unit { get; set; }
    }

    public class ProductSeed
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string Features { get; set; }
        public double Amount { get; set; }
        public string[] Urls { get; set; }
        public ICollection<PropertyValueSeed> Properties { get; set; }
    }

    public class PropertyValueSeed
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
