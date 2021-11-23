using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreItem> StoreItems { get; set; }
        public DbSet<Track> Tracks { get; set; }
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
                .Property(a => a.ConcurrencyStamp)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            builder.Entity<Address>()
                .HasOne(a => a.Area)
                .WithMany(l => l.Areas)
                .HasForeignKey(a => a.AreaId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Address>()
                .HasOne(a => a.City)
                .WithMany(l => l.Cities)
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Address>()
                .HasOne(a => a.State)
                .WithMany(l => l.States)
                .HasForeignKey(a => a.StateId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Address>()
                .HasOne(a => a.Country)
                .WithMany(l => l.Countries)
                .HasForeignKey(a => a.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Location>()
                .HasOne(l => l.Parent)
                .WithMany(l => l.Children)
                .HasForeignKey(l => l.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(p => p.Photo)
                .WithOne(p => p.Product)
                .HasForeignKey<Product>(p => p.PhotoId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
            builder.Entity<Product>()
                .HasMany(p => p.StoreItems)
                .WithOne(si => si.Product)
                .HasForeignKey(si => si.ProductId);

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

            builder.Entity<StoreItem>()
                .HasMany(si => si.OrderItems)
                .WithOne(oi => oi.StoreItem)
                .HasForeignKey(oi => oi.StoreItemId);
            builder.Entity<StoreItem>()
                .Property(si => si.ConcurrencyStamp)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            builder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Track>()
                .HasOne(t => t.Order)
                .WithOne(o => o.Track)
                .HasForeignKey<Track>(t => t.OrderId);
            builder.Entity<Track>()
                .HasOne(t => t.FromAddress)
                .WithMany(a => a.TracksFrom)
                .HasForeignKey(t => t.FromAddressId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Track>()
                .HasOne(t => t.ToAddress)
                .WithMany(a => a.TracksTo)
                .HasForeignKey(t => t.ToAddressId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Track>()
                .HasMany(t => t.Events)
                .WithOne(te => te.Track)
                .HasForeignKey(te => te.TrackId);

            builder.Entity<TrackEvent>()
                .HasOne(te => te.SiteLocation)
                .WithMany(l => l.TrackEvents)
                .HasForeignKey(te => te.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TrackEvent>()
                .HasOne(te => te.Agent)
                .WithMany(u => u.TrackEvents)
                .HasForeignKey(te => te.UserId)
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
        }
    }
}
