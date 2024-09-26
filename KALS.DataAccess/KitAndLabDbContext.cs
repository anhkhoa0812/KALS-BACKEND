using System.Reflection;
using KALS.Domain.Entity;
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
    public DbSet<Role> Role { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<ProductCategory> ProductCategory { get; set; }
    public DbSet<ProductRelationship> ProductRelationship { get; set; }
    public DbSet<Lab> Lab { get; set; }
    public DbSet<LabProduct> LabProduct { get; set; }
    public DbSet<Member> Member { get; set; }
    public DbSet<Payment> Payment { get; set; }
    public DbSet<Order> Order { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Role>().ToTable("Role");
        modelBuilder.Entity<Product>().ToTable("Product");
        modelBuilder.Entity<Category>().ToTable("Category");
        modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory");
        modelBuilder.Entity<ProductRelationship>().ToTable("ProductRelationship");
        modelBuilder.Entity<Lab>().ToTable("Lab");
        modelBuilder.Entity<LabProduct>().ToTable("LabProduct");
        modelBuilder.Entity<Member>().ToTable("Member");
        modelBuilder.Entity<Payment>().ToTable("Payment");
        modelBuilder.Entity<Order>().ToTable("Order");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=KALS;User Id=sa;Password=123456aA@$;Encrypt=True;TrustServerCertificate=True");
        }
    }
 }