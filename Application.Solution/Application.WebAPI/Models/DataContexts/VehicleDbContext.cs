using Application.WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Application.WebAPI.Models.DataContexts
{
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<Bus> Buses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            Type[] entities = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(BaseEntity))).ToArray();

            foreach (Type entity in entities)
            {
                builder.Entity(entity)
                       .Property("CreatedDate")
                       .HasDefaultValueSql("DATEADD(HOUR, 4, GETUTCDATE())");
            }
        }
    }
}
