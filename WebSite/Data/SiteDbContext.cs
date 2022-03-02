using Microsoft.EntityFrameworkCore;
using WebSite.Model;

namespace WebSite.Data;

public class SiteDbContext : DbContext
{
    public SiteDbContext(DbContextOptions<SiteDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        modelBuilder.Entity<SiteUser>()
            .HasIndex(u => u.UserName)
            .IsUnique();
    }

    public DbSet<SiteUser>? Users { get; set; }

    public DbSet<ForgotPassword>? ForgotPasswords { get; set; }

    public DbSet<ContactMessage>? ContactMessages { get; set; }
}