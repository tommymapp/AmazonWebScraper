using Api.Models;
using Api.UnitTests.Doubles;

namespace Api.UnitTests;

[TestClass]
public class AmazonWebScraperTests
{
    private readonly string _baseUrl = "amazon.co.uk";
    
    [TestMethod]
    public void Given_NoWatches_Then_DoesNotFetchWatchUrl()
    {
        var stubbedWatchRepo = new Stub_WatchRepo
        {
            WatchesToReturn = []
        };

        var spyWebClient = new SpyAmazonWebClient(_baseUrl);
        
        var scraper = new AmazonWebScraper(stubbedWatchRepo, spyWebClient);
        scraper.Start();

        Assert.HasCount(0, spyWebClient.RequestedUrls);
    }
    
    [TestMethod]
    [DataRow("TestIsRequested")]
    [DataRow("TestIsRequested2")]
    public void Given_OneWatch_Then_FetchesWatchUrl(string testPage)
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
        spyWebClient.StubbedPages.Add("TestIsRequested", "");
        
        var scraper = new AmazonWebScraper(stubbedWatchRepo, spyWebClient);
        scraper.Start();

        Assert.HasCount(1, spyWebClient.RequestedUrls);
        Assert.Contains($"{_baseUrl}/{testPage}", spyWebClient.RequestedUrls[0]);
    }

    [TestMethod]
    public void Given_MultipleWatches_Then_FetchWatchUrls()
    {
        var stubbedWatchRepo = new Stub_WatchRepo
        {
            WatchesToReturn =
            [
                new Watch(Guid.NewGuid(), $"{_baseUrl}/IsRequested1", 
                    100, "test@test.com", "Active", _baseUrl
                ),
                new Watch(Guid.NewGuid(), $"{_baseUrl}/IsRequested2", 
                    100, "test@test.com", "Active", _baseUrl
                ),
                new Watch(Guid.NewGuid(), $"{_baseUrl}/IsRequested3", 
                    100, "test@test.com", "Active", _baseUrl
                )
            ]
        };
    
        var spyWebClient = new SpyAmazonWebClient(_baseUrl);
        spyWebClient.StubbedPages.Add("TestIsRequested", "");
        
        var scraper = new AmazonWebScraper(stubbedWatchRepo, spyWebClient);
        scraper.Start();
    
        Assert.HasCount(3, spyWebClient.RequestedUrls);
        Assert.Contains($"{_baseUrl}/IsRequested1", spyWebClient.RequestedUrls);
        Assert.Contains($"{_baseUrl}/IsRequested2", spyWebClient.RequestedUrls);
        Assert.Contains($"{_baseUrl}/IsRequested3", spyWebClient.RequestedUrls);
    }
    
}