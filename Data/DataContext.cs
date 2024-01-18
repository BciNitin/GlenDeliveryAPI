using WebApplication1.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WebApplication1.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //modelBuilder.ApplyConfigurationsFromAssembly(typeof(Role).Assembly);
        //   // modelBuilder.Entity<Delivery>().OwnsOne(x => x.SalesOrderNo);
        //}
        //public DbSet<Delivery> Deliveryies { get; set; }
        public DbSet<Delivery> Delivery { get; set; } 
    }
}
