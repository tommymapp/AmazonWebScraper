using Api;
using Api.Exceptions;

namespace Api.AcceptanceTests.Features;

[TestClass]
public class AmazonPriceExtractionTests
{
    [TestMethod]
    [DataRow("DecimalNumber_Costs_83_30", 83.30)]
    [DataRow("DecimalNumber_Costs_117_57", 117.57)]
    public void Given_AmazonHTML_Then_ExtractsCorrectPrice(string htmlFile, double expectedPrice)
    {
        var html = File.ReadAllText(
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                "TestData", 
                $"{htmlFile}.html")
        );
        var priceParser = new AmazonPriceParser();
        var price = priceParser.GetPrice(html);
        Assert.AreEqual((decimal)expectedPrice, price);
    }

    [TestMethod]
    [DataRow("EUR_Costs_97_26", 97.26)]
    [DataRow("USD_Costs_52_57", 52.57)]
    public void Given_AmazonHTMLWithCurrency_Then_ExtractsCorrectPrice(string htmlFile, double expectedPrice)
    {
        var html = File.ReadAllText(
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                "TestData", 
                $"{htmlFile}.html")
        );
        
        var priceParser = new AmazonPriceParser();
        var price = priceParser.GetPrice(html);
        Assert.AreEqual((decimal)expectedPrice, price);
    }
    
    [TestMethod]
    [DataRow("HTML is empty", "")]
    [DataRow("HTML is different to expected", "<section>some other html</section>")]
    public void Given_BadHTMLString_Then_ThrowsPriceNotFoundException(string scenario, string html)
    {
        var priceParser = new AmazonPriceParser();
        Assert.Throws<PriceNotFoundException>(() => priceParser.GetPrice(html), $"Failed to throw when {scenario}");
    }
}