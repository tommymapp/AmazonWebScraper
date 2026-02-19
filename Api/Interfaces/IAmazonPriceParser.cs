namespace Api.Interfaces;

public interface IAmazonPriceParser
{
    public decimal GetPrice(string html);
}