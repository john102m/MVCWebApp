using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MVCWebApp.Areas.Services;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Data.SqlClient;
using MVCWebApp.Models;

namespace MVCWebApp.Models
{


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private ICyberSecurity _cyberSecurity;
        private IConfiguration _configuration;


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration, ICyberSecurity cyberSecurity)
            : base(options)
        {
            _cyberSecurity = cyberSecurity;
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
            options.UseSqlServer(_cyberSecurity.GetConnectionString(_configuration.GetConnectionString("LiveConnection")));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {


            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            // Customize the ASP.NET Core Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Core Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            //var converter = new ValueConverter<decimal, double>(
            //        v => (double)v,
            //        v => (decimal)v
            //);

            //builder
            //    .Entity<Movie>()
            //    .Property(e => e.Price)
            //    .HasConversion(converter);

        }

        public DbSet<Movie> Movie { get; set; }

        //public DbSet<MVCWebApp.Models.Todo> Todo { get; set; }
    }
}
