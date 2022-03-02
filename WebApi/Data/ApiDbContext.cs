using GanjAudio.WebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace GanjAudio.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>()
            .HasKey(n => n.Id);
    }

    public DbSet<Note> Notes { get; set; }
}