using Api.Contexts;
using Api.Interfaces;
using Api.Models;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

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

        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WatchDbContext>();

        context.Watches.AddRange(watches);
        await context.SaveChangesAsync();

        var scraper = scope.ServiceProvider.GetRequiredService<IAmazonWebScraper>();
         
        await scraper.Start();
         
        await Task.Delay(1000);

        var requests = WireMockServer?.LogEntries;
        var successfulBackgroundScraperTests = requests?.Where(r => r.RequestMessage.Path.Contains("BackgroundScraperTests_TestIsRequested"));
        Assert.HasCount(2, successfulBackgroundScraperTests!);
        Assert.IsFalse(requests?.Any(r => r.RequestMessage.Path.Contains("BackgroundScraperTests_TestIsNotRequested")));
    }
}