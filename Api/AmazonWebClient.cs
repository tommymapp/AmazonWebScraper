using Api.Interfaces;

namespace Api;

public class AmazonWebClient(HttpClient httpClient) : IAmazonWebClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> GetAmazonHtml(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}