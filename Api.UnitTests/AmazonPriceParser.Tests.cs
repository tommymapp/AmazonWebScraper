using Api;

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
}