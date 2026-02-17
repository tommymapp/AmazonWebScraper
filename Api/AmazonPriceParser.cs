using System.Text.RegularExpressions;
using Api.Exceptions;

namespace Api;

public class AmazonPriceParser
{
    public decimal GetPrice(string html)
    {
        if (html == "")
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