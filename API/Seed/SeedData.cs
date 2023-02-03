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

namespace API.Seed
{
    public class SeedData
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _config;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private static readonly Random Random = new();

        public string[] StoreAgents = { "Reed", "Carla", "Natsu", "Richard", "Asuna", "Gwen" };
        public string[] TrackAgents = { "Woods", "Izuku", "Miku", "Rem", "Casandra", "Peterson", "Elsa", "Victoria" };

        public SeedData(UserManager<User> userManager, RoleManager<Role> roleManager,
            IConfiguration config, DataContext context, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _context = context;
            _mapper = mapper;
        }

        public async Task SeedDatabase()
        {
            await _context.Database.MigrateAsync();
            await SeedRoles();
            await SeedLocation();
            await SeedUsers();
            await SeedTrackAgents();
            await SeedPayOption();
            await SeedStore();
            await SeedCategory();
            await SeedProduct();
        }

        async Task SeedRoles()
        {
            if (await _roleManager.Roles.AnyAsync()) return;

            foreach (var role in Enum.GetNames<RoleType>())
            {
                await _roleManager.CreateAsync(new Role { Name = role });
            }
        }

        async Task SeedUsers()
        {
            if (await _userManager.Users.AnyAsync()) return;

            var data = await File.ReadAllTextAsync("Seed/UserSeed.json");
            var users = JsonSerializer.Deserialize<List<User>>(data);

            if (users == null) return;

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                user.Account = new Account
                {
                    Balance = 100000
                };
                user.Address = await GetRandomAddress();
                user.Address.Name = user.Name;
                user.Address.Mobile = user.PhoneNumber;

                switch (user.UserName)
                {
                    case Constants.Admin:
                        user.Account.Balance = 2000000;
                        await _userManager.CreateAsync(user, _config["AdminPassword"]);
                        await _userManager.AddToRolesAsync(user, Enum.GetNames<RoleType>());
                        break;
                    case Constants.TestUser:
                        await _userManager.CreateAsync(user, _config["AdminPassword"]);
                        await _userManager.AddToRolesAsync(user, new[] { RoleType.User.ToString(), RoleType.TrackModerator.ToString(), RoleType.StoreModerator.ToString() });
                        break;
                    default:
                        await _userManager.CreateAsync(user, _config["AdminPassword"]);
                        await _userManager.AddToRoleAsync(user, RoleType.User.ToString());
                        break;
                }
            }
        }

        async Task SeedPayOption()
        {
            if (await _context.PayOptions.AnyAsync()) return;

            var payOptions = new List<PayOption>
            {
                new()
                {
                    Name = Constants.ShoppingCartWallet,
                    Available = true,
                    Type = PayType.Wallet
                },
                new()
                {
                    Name = PayType.DebitCard,
                    Type = PayType.DebitCard
                },
                new()
                {
                    Name = PayType.InternetBanking,
                    Type = PayType.InternetBanking
                },
                new()
                {
                    Name = PayType.Upi,
                    Type = PayType.Upi
                }
            };
            await _context.PayOptions.AddRangeAsync(payOptions);
            await _context.SaveChangesAsync();
        }

        async Task SeedStore()
        {
            if (await _context.Stores.AnyAsync()) return;

            var stores = new[] { "SuperComNet", "StoreEcom", "CORSECA", "PETILANTEOnline",
                "RetailNet", "AkshnavOnline", "OmniTechRetail", "IWQNBecommerce","RetailHomes","HomeKart" };

            var admin = await _context.Users.Where(u => u.UserName == "luffy").FirstAsync();

            foreach (var name in stores)
            {
                var moderatorUserName = StoreAgents[Random.Next(StoreAgents.Length)];
                var moderator = await _context.Users.Where(u => u.UserName == moderatorUserName.ToLower()).FirstAsync();

                var store = new Store
                {
                    Name = name,
                    Account = new Account(),
                    Address = await GetRandomAddress(),
                    StoreAgents = new List<StoreAgent>
                    {
                        new()
                        {
                            Role = RoleType.StoreAdmin.ToString(),
                            User = admin
                        },
                        new ()
                        {
                            Role = RoleType.StoreAgent.ToString(),
                            User = moderator
                        }
                    }
                };
                store.Address.Name = store.Name;
                await _context.Stores.AddAsync(store);
                await _context.SaveChangesAsync();
                await _userManager.AddToRoleAsync(moderator, RoleType.StoreAgent.ToString());
            }

            await _userManager.AddToRoleAsync(admin, RoleType.StoreAdmin.ToString());
        }

        async Task SeedLocation()
        {
            if (await _context.Locations.AnyAsync()) return;

            var data = await File.ReadAllTextAsync("Seed/LocationSeed.json");
            var country = JsonSerializer.Deserialize<Country>(data);
            var location = _mapper.Map<Location>(country);

            await _context.Locations.AddRangeAsync(location);
            await _context.SaveChangesAsync();
        }

        async Task SeedTrackAgents()
        {
            if (await _context.TrackAgents.AnyAsync()) return;

            var locations = await _context.Locations.ToListAsync();
            var admin = await _context.Users.Where(u => u.UserName == "zoro").FirstAsync();
            await _userManager.AddToRoleAsync(admin, RoleType.TrackAdmin.ToString());

            foreach (var item in locations)
            {
                var moderatorUserName = TrackAgents[Random.Next(TrackAgents.Length)];
                var moderator = await _context.Users.Where(u => u.UserName == moderatorUserName.ToLower()).FirstAsync();

                item.TrackAgents = new List<TrackAgent>
                {
                    new()
                    {
                        Location = item,
                        Role = RoleType.TrackAdmin.ToString(),
                        User = admin
                    },
                    new()
                    {
                        Location = item,
                        Role = RoleType.TrackAgent.ToString(),
                        User = moderator
                    }
                };
                await _context.SaveChangesAsync();
                await _userManager.AddToRoleAsync(moderator, RoleType.TrackAgent.ToString());
            }
        }

        async Task SeedCategory()
        {
            if (await _context.Categories.AnyAsync()) return;

            var data = await File.ReadAllTextAsync("Seed/CategorySeed.json");
            var productCategories = JsonSerializer.Deserialize<List<ProductCategory>>(data);
            var categories = new List<Category>();

            if (productCategories != null)
                categories.AddRange(productCategories.Select(_mapper.Map<Category>));

            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }

        async Task SeedProduct()
        {
            if (await _context.Products.AnyAsync()) return;

            var data = await File.ReadAllTextAsync("Seed/ProductSeed.json");
            var productSeeds = JsonSerializer.Deserialize<List<ProductSeed>>(data);

            if (productSeeds != null)
                foreach (var productSeed in productSeeds)
                {
                    var product = _mapper.Map<Product>(productSeed);
                    var category = await _context.Categories
                        .Include(c => c.Properties)
                        .Include(c => c.CategoryTags)
                        .SingleAsync(c => c.Name == productSeed.Category);
                    product.Category = category;
                    product.Properties = new List<PropertyValue>();
                    product.ProductViews = new List<ProductView>();
                    product.ProductTags = new List<ProductTag>();
                    product.SoldQuantity = Random.Next(1000, 100000);
                    product.Created = new DateTime(2021, 1, 1).AddDays(Random.Next(365));

                    foreach (var propertySeed in productSeed.Properties)
                    {
                        var property = category.Properties.Single(p => p.Name == propertySeed.Name);
                        var pv = new PropertyValue
                        {
                            Property = property
                        };

                        switch (property.Type)
                        {
                            case "String":
                                pv.StringValue = propertySeed.Value;
                                break;
                            case "Integer":
                                pv.IntegerValue = int.Parse(propertySeed.Value);
                                break;
                        }
                        product.Properties.Add(pv);
                    }

                    for (var i = 0; i < productSeed.Urls.Length; i++)
                    {
                        var productView = new ProductView
                        {
                            IsMain = i == 0,
                            Url = productSeed.Urls[i]
                        };
                        product.ProductViews.Add(productView);
                    }

                    for (var i = 0; i < productSeed.Tags.Length; i++)
                    {
                        var tag = new ProductTag
                        {
                            Name = productSeed.Tags[i].ToLower(),
                            Score = (short)(i == 0 ? 100 : 40)
                        };
                        product.ProductTags.Add(tag);
                    }

                    product.ProductTags.Add(new ProductTag { Name = product.Brand.ToLower(), Score = 15 });
                    product.ProductTags.Add(new ProductTag { Name = product.Model.ToLower(), Score = 100 });

                    foreach (var categoryTag in category.CategoryTags)
                    {
                        product.ProductTags.Add(new ProductTag
                        {
                            Name = categoryTag.Name.ToLower(),
                            Score = 10
                        });
                    }

                    if (!string.IsNullOrWhiteSpace(product.Model))
                    {
                        product.ProductTags.Add(new ProductTag
                        {
                            Name = product.Model.ToLower(),
                            Score = 100
                        });
                    }

                    product.MaxPerOrder = Random.Next(1, 5);
                    var stores = await _context.Stores.ToListAsync();
                    var storeItems = new List<StoreItem>();
                    foreach (var store in stores.OrderBy(_ => Guid.NewGuid()).Take(2))
                    {
                        storeItems.Add(new StoreItem
                        {
                            Store = store,
                            SoldQuantity = Random.Next(0, 1000),
                            Available = Random.Next(0, 1000)
                        });
                    }

                    product.Available = true;
                    product.StoreItems = storeItems;

                    await _context.Products.AddAsync(product);
                    await _context.SaveChangesAsync();
                }
        }

        async Task<Address> GetRandomAddress()
        {
            var count = await _context.Locations.Where(l => l.Type == "Area").CountAsync();
            var location = await _context.Locations
                .Where(l => l.Type == "Area")
                .Skip(Random.Next(0, count - 1))
                .FirstAsync();

            var landmarks = new[]
            {
                "Bull Temple", "Hotel Dwarka", "Vidyarthi Bhavan", "Maharaja Agrasen Hospital", "Church Parking",
                "Kanti Sweets", "Café Coffee Day", "Eden Park Restaurant", "Chaipoint", "Space Matrix",
                "Brundavan Cafe", "BMS College of Engineering", "Ashok Nagar Post Office"
            };

            var names = new[] { "Home", "Office", "Work", "College", "FarmHouse" };

            return new Address
            {
                Location = location,
                AddressName = names[Random.Next(0, names.Length)],
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
        public string Url { get; set; }
        public string[] Tags { get; set; }
        public List<Property> Properties { get; set; }
        public List<ProductCategory> SubCategories { get; set; }
    }

    public class Property
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Filter { get; set; } = true;
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
