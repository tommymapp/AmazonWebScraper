using Api.Contexts;
using Api.Interfaces;
using Api.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Api.AcceptanceTests.Features;

[TestClass]
public class BackgroundScraperTests : SystemTestBase
{
    [TestMethod]    
    public async Task Given_BackgroundWorkerTrigger_Then_FetchesHTMLForActiveWatchesOlderThan24Hours()
    {
        var watches = new Watch[]
        {
             new Watch(Guid.NewGuid(), $"{MockedAmazonUrl}/BackgroundScraperTests_TestIsRequested", 70, "test@test.com", "Active", MockedAmazonUrl!),
             new Watch(Guid.NewGuid(), $"{MockedAmazonUrl}/BackgroundScraperTests_TestIsNotRequested", 70, "test@test.com", "Active", MockedAmazonUrl!, DateTime.UtcNow - TimeSpan.FromHours(23) - TimeSpan.FromMinutes(58)),
             new Watch(Guid.NewGuid(), $"{MockedAmazonUrl}/BackgroundScraperTests_TestIsRequested", 70, "test@test.com", "Active", MockedAmazonUrl!, DateTime.UtcNow - TimeSpan.FromHours(24) - TimeSpan.FromMinutes(2)),
        };

        await SeedWatches(watches);

        using var scope = Factory.Services.CreateScope();
        var scraper = scope.ServiceProvider.GetRequiredService<IAmazonWebScraper>();
        await scraper.Start();
         
        await Task.Delay(1000);

        var requests = WireMockServer?.LogEntries;
        var successfulRequests = requests?.Where(r => r.RequestMessage.Path.Contains("BackgroundScraperTests_TestIsRequested"));
        
        Assert.IsNotNull(successfulRequests);
        Assert.AreEqual(2, successfulRequests.Count());
        Assert.IsFalse(requests?.Any(r => r.RequestMessage.Path.Contains("BackgroundScraperTests_TestIsNotRequested")));
    }

    [TestMethod]
    public async Task Given_ValidHtml_Then_PriceIsInsertedIntoDatabase()
    {
        var watch1 = new Watch(Guid.NewGuid(), $"{MockedAmazonUrl}/BackgroundScraperTests_Costs_83_30", 70, "test@test.com", "Active", MockedAmazonUrl!);
        var watch2 = new Watch(Guid.NewGuid(), $"{MockedAmazonUrl}/BackgroundScraperTests_Costs_117_57", 70, "test@test.com", "Active", MockedAmazonUrl!);

        await SeedWatches([watch1, watch2]);

        using (var scope = Factory.Services.CreateScope())
        {
            var scraper = scope.ServiceProvider.GetRequiredService<IAmazonWebScraper>();
            await scraper.Start();
        }

        await AssertPriceUpdated(watch1.Id, 83.30m);
        await AssertPriceUpdated(watch2.Id, 117.57m);
    }

    private async Task SeedWatches(Watch[] watches)
    {
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WatchDbContext>();
        context.Watches.AddRange(watches);
        await context.SaveChangesAsync();
    }

    private async Task AssertPriceUpdated(Guid id, decimal expectedPrice)
    {
        for (int i = 0; i < 10; i++)
        {
            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<WatchDbContext>();
            var watch = await db.Watches.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);

            if (watch?.Price == expectedPrice &&
                watch.LastChecked > DateTime.UtcNow.AddMinutes(-1)) 
                return;

            await Task.Delay(200);
        }

        Assert.Fail($"Price for {id} was not updated to {expectedPrice} with a fresh timestamp in time.");    }
}