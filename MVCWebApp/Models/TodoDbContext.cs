using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MVCWebApp.Models
{
    public class TodoDbContext : DbContext
    {
      
        private IConfiguration _configuration;

        public TodoDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
            options.UseSqlServer(_configuration.GetConnectionString("TodoConnection"));
        }
        public DbSet<Todo> TodoItems { get; set; }
    }
}
