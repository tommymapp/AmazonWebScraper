namespace Api.Models;

public class Watch
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; } = string.Empty;
    public decimal TargetPrice { get; set; }
    public decimal? Price { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    
    public DateTime? LastChecked { get; set; }

    public Watch()
    {
        
    }
    
    public Watch(Guid id, string url, decimal targetPrice, string email, string status, string baseUrl, DateTime? lastChecked = null)
    {
        Id = id;
        var baseUrlIndex = url.IndexOf(baseUrl);
        if (baseUrlIndex == -1)
            throw new ArgumentException("Base Url must be included in Url");
        
        var urlWithoutBaseAndProtocol = url.Substring(baseUrlIndex + baseUrl.Length);
        if(urlWithoutBaseAndProtocol[0] == '/')
            urlWithoutBaseAndProtocol = urlWithoutBaseAndProtocol.Substring(1); // strip trailing slash
        
        Url = urlWithoutBaseAndProtocol;
        TargetPrice = targetPrice;
        Email = email;
        Status = status;
        LastChecked = lastChecked;
    }
}