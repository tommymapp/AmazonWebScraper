using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Contexts;

public class WatchDbContext : DbContext
{
    public WatchDbContext(DbContextOptions<WatchDbContext> options)
    : base(options)
    {
        
    }
    
    public DbSet<Watch> Watches => Set<Watch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Watch>().ToTable("watches");
    }
}