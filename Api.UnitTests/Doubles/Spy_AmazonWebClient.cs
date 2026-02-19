using Api.Interfaces;

namespace Api.UnitTests.Doubles;

public class SpyAmazonWebClient(string baseUrl) : IAmazonWebClient
{
    private readonly string _baseUrl = baseUrl;
    public readonly Dictionary<string, string> StubbedPages = new Dictionary<string, string>();
    public readonly List<string> RequestedUrls = [];


    public async Task<string> GetAmazonHtml(string url)
    {
        RequestedUrls.Add($"{baseUrl}/{url}");
        return StubbedPages.TryGetValue(url, out var page) ?  page : "";
    }
}