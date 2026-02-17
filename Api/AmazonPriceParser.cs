using System.Text.RegularExpressions;

namespace Api;

public class AmazonPriceParser
{
    public decimal GetPrice(string html)
    {
        var regexPattern = "(?<=>)[0-9]+(?=<)";
        var matches = Regex.Matches(html, regexPattern);
        return decimal.Parse(
            matches.Count > 1 ?
                $"{matches[0].Value}.{matches[1].Value}"
                :
                matches[0].Value
            );
    }
}