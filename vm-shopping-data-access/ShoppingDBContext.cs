using Microsoft.EntityFrameworkCore;
using vm_shopping_data_access.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace vm_shopping_data_access
{
    public class ShoppingDBContext : DbContext
    {
        public ShoppingDBContext(DbContextOptions<ShoppingDBContext> options)
       : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<PaymentNotification> PaymentNotification { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Entity<Customer>()
                  .Property(c => c.Id)
                  .ValueGeneratedOnAdd();

                modelBuilder.Entity<Product>()
                   .Property(c => c.Id)
                   .ValueGeneratedOnAdd();

                modelBuilder.Entity<Status>();

                modelBuilder.Entity<Order>()
                 .Property(c => c.Id)
                 .ValueGeneratedOnAdd();

                modelBuilder.Entity<PaymentNotification>()
                  .Property(c => c.Id)
                  .ValueGeneratedOnAdd();
            }
            catch (Exception ex)
            { }
        }
    }
}
