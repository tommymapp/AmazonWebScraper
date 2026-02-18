using Api.Exceptions;

namespace Api.UnitTests;

[TestClass]
public class AmazonPriceParser_Tests
{
    [TestMethod]
    [DataRow("JustWholeNumber_Costs_76", 76)]
    [DataRow("JustWholeNumber_Costs_129", 129)]
    public void Given_AmazonHTML_Then_ExtractsCorrectPriceWithWholeNumbers(string htmlFile, int expectedPrice)
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
    [DataRow("DecimalNumber_Costs_83_30", 83.30)]
    [DataRow("DecimalNumber_Costs_117_57", 117.57)]
    public void Given_AmazonHTML_Then_ExtractsCorrectPriceWithDecimalNumbers(string htmlFile, double expectedPrice)
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
    public void Given_NoHTML_Then_ThrowsPriceNotFoundException()
    {
        var priceParser = new AmazonPriceParser();
        Assert.Throws<PriceNotFoundException>(() => priceParser.GetPrice(""));
    }
}