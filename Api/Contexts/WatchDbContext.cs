using Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Contexts;

public class WatchDbContext(DbContextOptions<WatchDbContext> options) : DbContext(options), IWatchRepo
{
    public DbSet<Watch> Watches => Set<Watch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Watch>().ToTable("watches");
    }

    public Watch[] GetActiveWatches()
    {
        return Watches.
            Where(w => 
                w.LastChecked == null 
                || 
                w.LastChecked <= DateTime.UtcNow.AddHours(-24))
            .ToArray();
    }

    public async Task UpdateWatch(Watch watch)
    {
        Watches.Update(watch);
        await SaveChangesAsync();
    }
}