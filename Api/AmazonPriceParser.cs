using System.Text.RegularExpressions;
using Api.Exceptions;
using Api.Interfaces;

namespace Api;

public class AmazonPriceParser : IAmazonPriceParser
{
    public decimal GetPrice(string html)
    {
        if (html == "")
            throw new PriceNotFoundException();


        var isUnavailablePattern = "(?<=primary-availability-message\">).+(?=</span>)";
        var matches = Regex.Matches(html, isUnavailablePattern);
        if (matches.Any(m => m.Value.Contains("Currently unavailable")))
            throw new PriceNotFoundException();

        var hasWholePricePattern = "(?<=\"a-price-whole\">)[0-9]+(?=<)";
        var wholePriceMatch = Regex.Match(html, hasWholePricePattern);
        if(!wholePriceMatch.Success) 
            throw new PriceNotFoundException();

        string? fractionVal = null;
        if (html.Contains("a-price-fraction"))
        {
            var hasFractionPricePattern = "(?<=\"a-price-fraction\">)[0-9]+(?=<)";
            var fractionPriceMatch = Regex.Match(html, hasFractionPricePattern);
            if(!fractionPriceMatch.Success) 
                throw new PriceNotFoundException();    
            
            fractionVal = fractionPriceMatch.Value;
        }
        
        return decimal.Parse(
            fractionVal != null ?
                $"{wholePriceMatch.Value}.{fractionVal}"
                :
                wholePriceMatch.Value
            );
    }
}