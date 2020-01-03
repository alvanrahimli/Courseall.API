using CourseAll.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseAll.API.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Courseall.db");
        }

        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Service> Services { get; set; }
    }
}