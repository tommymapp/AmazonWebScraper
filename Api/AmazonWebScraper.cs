using Api.Interfaces;
using Api.Models;

namespace Api;

public class AmazonWebScraper(IWatchRepo watchRepo, IAmazonWebClient webClient) : IAmazonWebScraper
{
    private readonly IWatchRepo _watchRepo = watchRepo;
    private readonly IAmazonWebClient _webClient = webClient;

    public async Task Start()
    {
        var watches = _watchRepo.GetActiveWatches();
        
        foreach (var watch in watches)
        {
            await _webClient.GetAmazonHtml(watch.Url);
        }
    }
}