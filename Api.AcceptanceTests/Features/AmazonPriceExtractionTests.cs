using Api;
using Api.Exceptions;

namespace Api.AcceptanceTests.Features;

[TestClass]
public class AmazonPriceExtractionTests
{
    [TestMethod]
    [DataRow("<span class=\"a-price-whole\">89<span class=\"a-price-decimal\">.</span></span>", 89)]
    [DataRow("<span class=\"a-price-whole\">123<span class=\"a-price-decimal\">.</span></span>", 123)]
    public void Given_AmazonHTML_Then_ExtractsCorrectPrice(string html, double expectedPrice)
    {
        var priceParser = new AmazonPriceParser();
        var price = priceParser.GetPrice(html);
        Assert.AreEqual((decimal)expectedPrice, price);
    }

    [TestMethod]
    [DataRow("<span class=\"a-price-symbol\">£</span><span class=\"a-price-whole\">139<span class=\"a-price-decimal\">.</span></span><span class=\"a-price-fraction\">99</span>", 139.99)]
    [DataRow("<span class=\"a-price-symbol\">$</span><span class=\"a-price-whole\">12<span class=\"a-price-decimal\">.</span></span><span class=\"a-price-fraction\">99</span>", 12.99)]
    public void Given_AmazonHTMLWithCurrency_Then_ExtractsCorrectPrice(string html, double expectedPrice)
    {
        var priceParser = new AmazonPriceParser();
        var price = priceParser.GetPrice(html);
        Assert.AreEqual((decimal)expectedPrice, price);
    }
    
    [TestMethod]
    [DataRow("HTML is empty", "")]
    [DataRow("HTML is different to expected", "<section>some other html</section>")]
    [DataRow("No whole price", "<span class=\"a-price-whole\"><span class=\"a-price-decimal\">.</span></span>")]
    [DataRow("Whole price is non-number", "<span class=\"a-price-whole\">abc<span class=\"a-price-decimal\">.</span></span>")]
    [DataRow("Fraction price is non-number", "<span class=\"a-price-whole\">139<span class=\"a-price-decimal\">.</span></span><span class=\"a-price-fraction\">test</span>")]
    public void Given_BadHTMLString_Then_ThrowsPriceNotFoundException(string scenario, string html)
    {
        var priceParser = new AmazonPriceParser();
        Assert.Throws<PriceNotFoundException>(() => priceParser.GetPrice(html), $"Failed to throw when {scenario}");
    }
}