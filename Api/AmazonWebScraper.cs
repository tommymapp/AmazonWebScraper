using Api.Interfaces;
using Api.Models;

namespace Api;

public class AmazonWebScraper(IWatchRepo watchRepo, IAmazonWebClient webClient, IAmazonPriceParser priceParser) : IAmazonWebScraper
{
    public async Task Start()
    {
        var watches = watchRepo.GetActiveWatches();
        
        foreach (var watch in watches)
        {
            var html = await webClient.GetAmazonHtml(watch.Url);
            var price = priceParser.GetPrice(html);
            watch.Price = price;
            await watchRepo.UpdateWatch(watch);
        }
    }
}