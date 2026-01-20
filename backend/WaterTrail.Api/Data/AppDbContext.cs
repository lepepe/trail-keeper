using Microsoft.EntityFrameworkCore;
using WaterTrail.Api.Models;

namespace WaterTrail.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<Point> Points => Set<Point>();
    public DbSet<RoutePath> RoutePaths => Set<RoutePath>();
    public DbSet<Gear> Gear => Set<Gear>();
    public DbSet<Food> Food => Set<Food>();
    public DbSet<Night> Nights => Set<Night>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map to existing table names (lowercase)
        modelBuilder.Entity<Trip>(entity =>
        {
            entity.ToTable("trips");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Type).HasColumnName("type").HasConversion<string>();
        });

        modelBuilder.Entity<Point>(entity =>
        {
            entity.ToTable("points");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TripId).HasColumnName("trip_id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(200);
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.Icon).HasColumnName("icon");
            entity.Property(e => e.Color).HasColumnName("color");
            entity.HasOne(e => e.Trip)
                  .WithMany(t => t.Points)
                  .HasForeignKey(e => e.TripId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RoutePath>(entity =>
        {
            entity.ToTable("paths");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TripId).HasColumnName("trip_id");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.HasOne(e => e.Trip)
                  .WithMany(t => t.Paths)
                  .HasForeignKey(e => e.TripId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Gear>(entity =>
        {
            entity.ToTable("gear");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TripId).HasColumnName("trip_id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(200);
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Packed).HasColumnName("packed");
            entity.HasOne(e => e.Trip)
                  .WithMany(t => t.Gear)
                  .HasForeignKey(e => e.TripId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.ToTable("food");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TripId).HasColumnName("trip_id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(200);
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Eaten).HasColumnName("eaten");
            entity.HasOne(e => e.Trip)
                  .WithMany(t => t.Food)
                  .HasForeignKey(e => e.TripId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Night>(entity =>
        {
            entity.ToTable("nights");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TripId).HasColumnName("trip_id");
            entity.Property(e => e.NightNumber).HasColumnName("night_number");
            entity.Property(e => e.Campsite).HasColumnName("campsite");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.HasOne(e => e.Trip)
                  .WithMany(t => t.Nights)
                  .HasForeignKey(e => e.TripId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        // Just ensure database exists, don't create tables (they already exist)
        // Seeding is skipped if data already exists
        if (context.Trips.Any())
        {
            return;
        }

        // Seed only if empty
        var trip = new Trip
        {
            Name = "Suwannee River Kayak Adventure",
            Description = "A scenic kayak route along the Suwannee River.",
            Type = TripType.Kayak
        };

        context.Trips.Add(trip);
        context.SaveChanges();

        var points = new List<Point>
        {
            new() { TripId = trip.Id, Name = "Lafayette Blue Springs State Park (Night 1, Mile 103.3)", Latitude = 30.1272, Longitude = -83.2255, Icon = "fa-campground", Color = "green" },
            new() { TripId = trip.Id, Name = "Peacock Slough River Camp (Night 2, Mile 95.8)", Latitude = 30.1024, Longitude = -83.1383, Icon = "fa-campground", Color = "green" },
            new() { TripId = trip.Id, Name = "Patrician Oaks (Take-Out, Mile 79.0)", Latitude = 29.9771, Longitude = -82.9615, Icon = "fa-anchor", Color = "red" }
        };
        context.Points.AddRange(points);

        var pathCoords = new double[,]
        {
            {30.2463, -83.2461}, {30.2300, -83.2450}, {30.2100, -83.2400}, {30.1800, -83.2350},
            {30.1500, -83.2300}, {30.1272, -83.2255}, {30.1200, -83.2100}, {30.1150, -83.2000},
            {30.1100, -83.1850}, {30.1050, -83.1650}, {30.1030, -83.1500}, {30.1024, -83.1383},
            {30.1000, -83.1250}, {30.0900, -83.1100}, {30.0800, -83.0900}, {30.0650, -83.0650},
            {30.0550, -83.0450}, {30.0450, -83.0300}, {30.0352, -83.0189}, {30.0250, -83.0100},
            {30.0150, -83.0000}, {30.0050, -82.9900}, {29.9950, -82.9800}, {29.9850, -82.9700},
            {29.9771, -82.9615}
        };

        for (int i = 0; i < pathCoords.GetLength(0); i++)
        {
            context.RoutePaths.Add(new RoutePath
            {
                TripId = trip.Id,
                Latitude = pathCoords[i, 0],
                Longitude = pathCoords[i, 1]
            });
        }

        var gearItems = new List<Gear>
        {
            new() { TripId = trip.Id, Name = "Kayak", Quantity = 1, Packed = true },
            new() { TripId = trip.Id, Name = "Paddle", Quantity = 1, Packed = true },
            new() { TripId = trip.Id, Name = "Life Jacket", Quantity = 1, Packed = true },
            new() { TripId = trip.Id, Name = "Tent", Quantity = 1, Packed = false },
            new() { TripId = trip.Id, Name = "Sleeping Bag", Quantity = 1, Packed = false },
            new() { TripId = trip.Id, Name = "Sleeping Pad", Quantity = 1, Packed = false },
            new() { TripId = trip.Id, Name = "Cooking Stove", Quantity = 1, Packed = false },
            new() { TripId = trip.Id, Name = "Fuel", Quantity = 1, Packed = false },
            new() { TripId = trip.Id, Name = "Cookware", Quantity = 1, Packed = false },
            new() { TripId = trip.Id, Name = "Water Filter", Quantity = 1, Packed = false },
            new() { TripId = trip.Id, Name = "First Aid Kit", Quantity = 1, Packed = true },
            new() { TripId = trip.Id, Name = "Headlamp", Quantity = 1, Packed = true },
            new() { TripId = trip.Id, Name = "Dry Bags", Quantity = 3, Packed = false }
        };
        context.Gear.AddRange(gearItems);

        var foodItems = new List<Food>
        {
            new() { TripId = trip.Id, Name = "Oatmeal", Quantity = 2, Eaten = false },
            new() { TripId = trip.Id, Name = "Dehydrated Meals", Quantity = 4, Eaten = false },
            new() { TripId = trip.Id, Name = "Energy Bars", Quantity = 6, Eaten = false },
            new() { TripId = trip.Id, Name = "Coffee", Quantity = 1, Eaten = false },
            new() { TripId = trip.Id, Name = "Water", Quantity = 1, Eaten = false }
        };
        context.Food.AddRange(foodItems);

        var nights = new List<Night>
        {
            new() { TripId = trip.Id, NightNumber = 1, Campsite = "Lafayette Blue Springs State Park", Notes = "Arrive before 5pm to set up camp." },
            new() { TripId = trip.Id, NightNumber = 2, Campsite = "Peacock Slough River Camp", Notes = "Primitive campsite, no facilities." }
        };
        context.Nights.AddRange(nights);

        context.SaveChanges();
    }
}
