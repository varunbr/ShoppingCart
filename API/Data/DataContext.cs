using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTag> CategoryTags { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PayOption> PayOptions { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductView> ProductViews { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyValue> PropertyValues { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreAgent> StoresAgents { get; set; }
        public DbSet<StoreItem> StoreItems { get; set; }
        public DbSet<TrackAgent> TrackAgents { get; set; }
        public DbSet<TrackEvent> TrackEvents { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<User>(u => u.AddressId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<User>()
                .HasOne(u => u.Account)
                .WithOne(a => a.User)
                .HasForeignKey<User>(u => u.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<User>()
                .HasOne(u => u.Photo)
                .WithOne(p => p.User)
                .HasForeignKey<User>(u => u.PhotoId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            builder.Entity<Role>()
                .HasMany(ar => ar.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId);

            builder.Entity<User>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);

            builder.Entity<Store>()
                .HasOne(s => s.Address)
                .WithOne(a => a.Store)
                .HasForeignKey<Store>(s => s.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Store>()
                .HasOne(s => s.Account)
                .WithOne(a => a.Store)
                .HasForeignKey<Store>(s => s.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Store>()
                .HasOne(s => s.Photo)
                .WithOne(p => p.Store)
                .HasForeignKey<Store>(s => s.PhotoId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Store>()
                .HasMany(s => s.Items)
                .WithOne(si => si.Store)
                .HasForeignKey(si => si.StoreId);
            builder.Entity<Store>()
                .HasMany(s => s.Orders)
                .WithOne(o => o.Store)
                .HasForeignKey(o => o.StoreId);

            builder.Entity<StoreAgent>()
                .HasOne(sa => sa.User)
                .WithMany(u => u.StoreAgents)
                .HasForeignKey(sa => sa.UserId);
            builder.Entity<StoreAgent>()
                .HasOne(sa => sa.Store)
                .WithMany(s => s.StoreAgents)
                .HasForeignKey(sa => sa.StoreId);

            builder.Entity<Account>()
                .HasMany(a => a.Deposit)
                .WithOne(t => t.ToAccount)
                .HasForeignKey(t => t.ToId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Account>()
                .HasMany(a => a.Withdraw)
                .WithOne(t => t.FromAccount)
                .HasForeignKey(t => t.FromId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Account>()
                .Property(a => a.RowVersion)
                .IsRowVersion();

            builder.Entity<Address>()
                .HasOne(a => a.Location)
                .WithMany(l => l.Addresses)
                .HasForeignKey(a => a.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Location>()
                .HasOne(l => l.Parent)
                .WithMany(l => l.Children)
                .HasForeignKey(l => l.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
            builder.Entity<Product>()
                .HasMany(p => p.StoreItems)
                .WithOne(si => si.Product)
                .HasForeignKey(si => si.ProductId);
            builder.Entity<Product>()
                .HasMany(p => p.Properties)
                .WithOne(pv => pv.Product)
                .HasForeignKey(pv => pv.ProductId);
            builder.Entity<Product>()
                .HasMany(p => p.ProductTags)
                .WithOne(t => t.Product)
                .HasForeignKey(t => t.ProductId);
            builder.Entity<Product>()
                .Property(p => p.RowVersion)
                .IsRowVersion();

            builder.Entity<ProductView>()
                .HasOne(pv => pv.Product)
                .WithMany(p => p.ProductViews);

            builder.Entity<Category>()
                .HasMany(c => c.Children)
                .WithOne(c => c.Parent)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Category>()
                .HasOne(c => c.Photo)
                .WithOne(p => p.Category)
                .HasForeignKey<Category>(c => c.PhotoId);
            builder.Entity<Category>()
                .HasMany(c => c.Properties)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);
            builder.Entity<Category>()
                .HasMany(c => c.CategoryTags)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId);

            builder.Entity<Property>()
                .HasMany(p => p.PropertyValues)
                .WithOne(pv => pv.Property)
                .HasForeignKey(pv => pv.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StoreItem>()
                .HasMany(si => si.OrderItems)
                .WithOne(oi => oi.StoreItem)
                .HasForeignKey(oi => oi.StoreItemId);
            builder.Entity<StoreItem>()
                .Property(si => si.RowVersion)
                .IsRowVersion();

            builder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Order>()
                .HasOne(o => o.Transaction)
                .WithOne(t => t.Order)
                .HasForeignKey<Order>(o => o.TransactionId);

            builder.Entity<Order>()
                .HasOne(o => o.DestinationLocation)
                .WithMany(l => l.DestinationOrders)
                .HasForeignKey(o => o.DestinationLocationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Order>()
                .HasOne(o => o.SourceLocation)
                .WithMany(l => l.SourceOrders)
                .HasForeignKey(o => o.SourceLocationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Order>()
                .HasMany(o => o.TrackEvents)
                .WithOne(te => te.Order)
                .HasForeignKey(te => te.OrderId);

            builder.Entity<TrackEvent>()
                .HasOne(te => te.SiteLocation)
                .WithMany(l => l.TrackEvents)
                .HasForeignKey(te => te.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TrackEvent>()
                .HasOne(te => te.Agent)
                .WithMany(u => u.TrackEvents)
                .HasForeignKey(te => te.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TrackAgent>()
                .HasOne(ta => ta.User)
                .WithMany(u => u.TrackAgents)
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TrackAgent>()
                .HasOne(ta => ta.Location)
                .WithMany(l => l.TrackAgents)
                .HasForeignKey(ta => ta.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(c => c.UserId);
            builder.Entity<CartItem>()
                .HasOne(c => c.StoreItem)
                .WithMany(si => si.CartItems)
                .HasForeignKey(c => c.StoreItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
