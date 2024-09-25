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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Role>().ToTable("Role");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=KALS;User Id=sa;Password=123456aA@$;Encrypt=True;TrustServerCertificate=True");
        }
    }
 }