using Application.WebAPI.Models.Entities;
using Application.WebAPI.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Application.WebAPI.Models.DataContexts
{
    public class VehicleDbContext : IdentityDbContext<VehicleUser, VehicleRole, int, VehicleUserClaim,
        VehicleUserRole, VehicleUserLogin, VehicleRoleClaim, VehicleUserToken>
    {
        public VehicleDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<Bus> Buses { get; set; }

        //Identity
        public DbSet<VehicleUser> Users { get; set; }
        public DbSet<VehicleRole> Roles { get; set; }
        public DbSet<VehicleUserRole> UserRoles { get; set; }
        public DbSet<VehicleUserClaim> UserClaims { get; set; }
        public DbSet<VehicleRoleClaim> RoleClaims { get; set; }
        public DbSet<VehicleUserLogin> UserLogins { get; set; }
        public DbSet<VehicleUserToken> UserTokens { get; set; }
        //Identity

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

            builder.Entity<VehicleUser>(cfg =>
            {
                cfg.ToTable("Users", "Membership");
            });

            builder.Entity<VehicleRole>(cfg =>
            {
                cfg.ToTable("Roles", "Membership");
            });

            builder.Entity<VehicleUserRole>(cfg =>
            {
                cfg.ToTable("UserRoles", "Membership");
            });

            builder.Entity<VehicleUserClaim>(cfg =>
            {
                cfg.ToTable("UserClaims", "Membership");
            });

            builder.Entity<VehicleRoleClaim>(cfg =>
            {
                cfg.ToTable("RoleClaims", "Membership");
            });

            builder.Entity<VehicleUserLogin>(cfg =>
            {
                cfg.ToTable("UserLogins", "Membership");
            });

            builder.Entity<VehicleUserToken>(cfg =>
            {
                cfg.ToTable("UserTokens", "Membership");
            });
        }
    }
}
