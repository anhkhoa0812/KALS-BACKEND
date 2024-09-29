using System.Reflection;
using KALS.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KALS.Domain.DataAccess;

public class KitAndLabDbContext : DbContext
{
    public KitAndLabDbContext()
    {
    }
    public KitAndLabDbContext(DbContextOptions<KitAndLabDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> User { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<ProductCategory> ProductCategory { get; set; }
    public DbSet<Lab> Lab { get; set; }
    public DbSet<LabProduct> LabProduct { get; set; }
    public DbSet<Member> Member { get; set; }
    public DbSet<Staff> Staff { get; set; } 
    public DbSet<Payment> Payment { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderItem> OrderItem { get; set; }
    public DbSet<SupportRequest> SupportRequest { get; set; }
    public DbSet<SupportMessage> SupportMessage { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Product>().ToTable("Product");
        modelBuilder.Entity<Category>().ToTable("Category");
        modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory");
        modelBuilder.Entity<Lab>().ToTable("Lab");
        modelBuilder.Entity<LabProduct>().ToTable("LabProduct");
        modelBuilder.Entity<Member>().ToTable("Member");
        modelBuilder.Entity<Staff>().ToTable("Staff");
        modelBuilder.Entity<Payment>().ToTable("Payment");
        modelBuilder.Entity<Order>().ToTable("Order");
        modelBuilder.Entity<OrderItem>().ToTable("OrderItem");
        modelBuilder.Entity<SupportRequest>().ToTable("SupportRequest");
        modelBuilder.Entity<SupportMessage>().ToTable("SupportMessage");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=KALS;User Id=sa;Password=123456aA@$;Encrypt=True;TrustServerCertificate=True");
        }
    }
 }