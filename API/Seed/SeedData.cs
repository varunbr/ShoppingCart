using API.Data;
using API.Entities;
using API.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace API.Seed
{
    public class SeedData
    {
        private static readonly Random Random = new();

        public static async Task SeedRoles(RoleManager<IdentityRole<int>> roleManager)
        {
            if (await roleManager.Roles.AnyAsync()) return;

            foreach (var role in Enum.GetNames<Role>())
            {
                await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
            }
        }

        public static async Task SeedUsers(UserManager<User> userManager, DataContext context, IConfiguration config)
        {
            if (await userManager.Users.AnyAsync()) return;

            var data = await File.ReadAllTextAsync("Seed/UserSeed.json");
            var users = JsonSerializer.Deserialize<List<User>>(data);

            if (users == null) return;

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                user.Account = new Account
                {
                    Balance = 1000
                };
                user.Address = await GetRandomAddress(context);

                if (user.UserName == "admin")
                {
                    await userManager.CreateAsync(user, config["AdminPassword"]);
                    await userManager.AddToRolesAsync(user, new[] { Role.User.ToString(), Role.Admin.ToString() });
                    continue;
                }

                await userManager.CreateAsync(user, "User@2021");
                await userManager.AddToRoleAsync(user, Role.User.ToString());
            }
        }

        public static async Task SeedStore(DataContext context)
        {
            if (await context.Stores.AnyAsync()) return;

            var stores = new[] { "SuperComNet", "StoreEcom", "CORSECA", "PETILANTE Online",
                "RetailNet", "Akshnav Online", "OmniTechRetail", "IWQNBecommerce","RetailHomes","HomeKart" };

            foreach (var name in stores)
            {
                var store = new Store
                {
                    Name = name,
                    Account = new Account { Balance = 10000 },
                    Address = await GetRandomAddress(context)
                };
                await context.Stores.AddAsync(store);
                await context.SaveChangesAsync();
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
                        .Include(c => c.Properties)
                        .SingleAsync(c => c.Name == productSeed.Category);
                    product.Category = category;
                    product.Properties = new List<PropertyValue>();
                    product.ProductViews = new List<ProductView>();
                    product.ProductTags = new List<ProductTag>();
                    product.SoldQuantity = Random.Next(1000, 100000);
                    product.Created=new DateTime(2021,1,1).AddDays(Random.Next(365));

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
                            IsMain = i == 0,
                            Photo = new Photo
                            {
                                Url = productSeed.Urls[i]
                            }
                        };
                        product.ProductViews.Add(productView);
                    }

                    for (var i = 0; i < productSeed.Tags.Length; i++)
                    {
                        var tag = new ProductTag
                        {
                            Name = productSeed.Tags[i],
                            Score = (short)(i == 0 ? 100 : 40)
                        };
                        product.ProductTags.Add(tag);
                    }
                    if (!string.IsNullOrWhiteSpace(product.Model))
                    {
                        product.ProductTags.Add(new ProductTag
                        {
                            Name = product.Model,
                            Score = 100
                        });
                    }

                    var maxPreOrder = Random.Next(1, 5);
                    var stores = await context.Stores.OrderBy(s => Guid.NewGuid()).Take(2).ToListAsync();
                    var storeItems = new List<StoreItem>();
                    foreach (var store in stores)
                    {
                        storeItems.Add(new StoreItem
                        {
                            Store = store,
                            Count = Random.Next(10, 1000),
                            MaxPerOrder = maxPreOrder
                        });
                    }
                    product.StoreItems = storeItems;

                    await context.Products.AddAsync(product);
                    await context.SaveChangesAsync();
                }
        }

        private static async Task<Address> GetRandomAddress(DataContext context)
        {
            var location = await context.Locations
                .Where(l => l.Type == "Area")
                .OrderBy(l => Guid.NewGuid())
                .FirstAsync();

            var landmarks = new[]
            {
                "Bull Temple", "Hotel Dwarka", "Vidyarthi Bhavan", "Maharaja Agrasen Hospital", "Church Parking",
                "Kanti Sweets", "Café Coffee Day", "Eden Park Restaurant", "Chaipoint", "Space Matrix",
                "Brundavan Cafe", "BMS College of Engineering", "Ashok Nagar Post Office"
            };

            return new Address
            {
                Location = location,
                House = $"#{Random.Next(1, 100)}, {Random.Next(3, 26)}th Cross, {Random.Next(3, 15)}th Main",
                Landmark = landmarks[Random.Next(0, landmarks.Length)],
                PostalCode = Random.Next(500000, 599999).ToString()
            };
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
        public string[] Tags { get; set; }
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
        public string[] Tags { get; set; }
        public string[] Urls { get; set; }
        public ICollection<PropertyValueSeed> Properties { get; set; }
    }

    public class PropertyValueSeed
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
