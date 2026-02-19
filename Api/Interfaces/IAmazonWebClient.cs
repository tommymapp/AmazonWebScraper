namespace Api.Interfaces;

public interface IAmazonWebClient
{
    public Task<string> GetAmazonHtml(string url);
}