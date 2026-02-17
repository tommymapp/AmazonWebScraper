using Api.Exceptions;

namespace Api.UnitTests;

[TestClass]
public class AmazonPriceParser_Tests
{
    [TestMethod]
    [DataRow("<span class=\"a-price-whole\">89<span class=\"a-price-decimal\">.</span></span>", 89)]
    [DataRow("<span class=\"a-price-whole\">154<span class=\"a-price-decimal\">.</span></span>", 154)]
    public void Given_AmazonHTML_Then_ExtractsCorrectPriceWithWholeNumbers(string html, int expectedPrice)
    {
        var priceParser = new AmazonPriceParser();
        var price = priceParser.GetPrice(html);
        Assert.AreEqual((decimal)expectedPrice, price);
    }

    [TestMethod]
    [DataRow("<span class=\"a-price-whole\">89<span class=\"a-price-decimal\">.</span></span><span class=\"a-price-fraction\">56</span>", 89.56)]
    [DataRow("<span class=\"a-price-whole\">154<span class=\"a-price-decimal\">.</span></span><span class=\"a-price-fraction\">18</span>", 154.18)]
    public void Given_AmazonHTML_Then_ExtractsCorrectPriceWithDecimalNumbers(string html, double expectedPrice)
    {
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
    
    [TestMethod]
    public void Given_NoWholePrice_Then_ThrowsPriceNotFoundException()
    {
        var priceParser = new AmazonPriceParser();
        var html = "<span class=\"a-price-whole\"><span class=\"a-price-decimal\">.</span></span>";
        Assert.Throws<PriceNotFoundException>(() => priceParser.GetPrice(html));
    }
    
    [TestMethod]
    public void Given_AlphaWholePrice_Then_ThrowsPriceNotFoundException()
    {
        var priceParser = new AmazonPriceParser();
        var html = "<span class=\"a-price-whole\">test<span class=\"a-price-decimal\">.</span></span>";
        Assert.Throws<PriceNotFoundException>(() => priceParser.GetPrice(html));
    }
    
    [TestMethod]
    public void Given_AlphaFractionPrice_Then_ThrowsPriceNotFoundException()
    {
        var priceParser = new AmazonPriceParser();
        var html = "<span class=\"a-price-whole\">89<span class=\"a-price-decimal\">.</span></span><span class=\"a-price-fraction\">test</span>";
        Assert.Throws<PriceNotFoundException>(() => priceParser.GetPrice(html));
    }
}