using Api.Models;
using Api.UnitTests.Doubles;

namespace Api.UnitTests;

[TestClass]
public class AmazonWebScraperTests
{
    private readonly string _baseUrl = "amazon.co.uk";
    
    [TestMethod]
    public async Task Given_NoWatches_Then_DoesNotFetchWatchUrl()
    {
        var stubbedWatchRepo = new Stub_WatchRepo
        {
            WatchesToReturn = []
        };

        var spyWebClient = new SpyAmazonWebClient(_baseUrl);

        var priceParser = new AmazonPriceParser();
        var scraper = new AmazonWebScraper(stubbedWatchRepo, spyWebClient, priceParser);
        await scraper.Start();

        Assert.HasCount(0, spyWebClient.RequestedUrls);
    }
    
    [TestMethod]
    [DataRow("IsRequested1")]
    [DataRow("IsRequested2")]
    public async Task Given_OneWatch_Then_FetchesWatchUrl(string testPage)
    {
        var stubbedWatchRepo = new Stub_WatchRepo
        {
            WatchesToReturn =
            [
                new Watch(Guid.NewGuid(), $"{_baseUrl}/{testPage}", 
                    100, "test@test.com", "Active", _baseUrl
                )
            ]
        };

        var spyWebClient = new SpyAmazonWebClient(_baseUrl);
        spyWebClient.StubbedPages.Add(testPage, "<span class=\"a-price-whole\">83<span class=\"a-price-decimal\">.</span>");
        
        var priceParser = new AmazonPriceParser();
        var scraper = new AmazonWebScraper(stubbedWatchRepo, spyWebClient, priceParser);
        await scraper.Start();

        Assert.HasCount(1, spyWebClient.RequestedUrls);
        Assert.Contains($"{_baseUrl}/{testPage}", spyWebClient.RequestedUrls[0]);
    }

    [TestMethod]
    public async Task Given_MultipleWatches_Then_FetchWatchUrls()
    {
        var stubbedWatchRepo = new Stub_WatchRepo
        {
            WatchesToReturn =
            [
                new Watch(Guid.NewGuid(), $"{_baseUrl}/AmazonWebScraper_TestIsRequested1", 
                    100, "test@test.com", "Active", _baseUrl
                ),
                new Watch(Guid.NewGuid(), $"{_baseUrl}/AmazonWebScraper_TestIsRequested2", 
                    100, "test@test.com", "Active", _baseUrl
                ),
                new Watch(Guid.NewGuid(), $"{_baseUrl}/AmazonWebScraper_TestIsRequested3", 
                    100, "test@test.com", "Active", _baseUrl
                )
            ]
        };
    
        var spyWebClient = new SpyAmazonWebClient(_baseUrl);
        spyWebClient.StubbedPages.Add("AmazonWebScraper_TestIsRequested1", "<span class=\"a-price-whole\">83<span class=\"a-price-decimal\">.</span>");
        spyWebClient.StubbedPages.Add("AmazonWebScraper_TestIsRequested2", "<span class=\"a-price-whole\">83<span class=\"a-price-decimal\">.</span>");
        spyWebClient.StubbedPages.Add("AmazonWebScraper_TestIsRequested3", "<span class=\"a-price-whole\">83<span class=\"a-price-decimal\">.</span>");
        
        var priceParser = new AmazonPriceParser();
        var scraper = new AmazonWebScraper(stubbedWatchRepo, spyWebClient, priceParser);
        await scraper.Start();
    
        Assert.Contains($"{_baseUrl}/AmazonWebScraper_TestIsRequested1", spyWebClient.RequestedUrls);
        Assert.Contains($"{_baseUrl}/AmazonWebScraper_TestIsRequested2", spyWebClient.RequestedUrls);
        Assert.Contains($"{_baseUrl}/AmazonWebScraper_TestIsRequested3", spyWebClient.RequestedUrls);
    }
    
}