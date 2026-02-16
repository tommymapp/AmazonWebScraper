using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Api.AcceptanceTests.Features;

[TestClass]
public class CreateWatchTests : BaseAcceptanceTest
{
    [TestMethod]
    public async Task Given_Amazon_URL_Returns_Status_201()
    {
        var request = new
        {
            Url = "https://amazon.co.uk/Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB",
            TargetPrice = 100,
            Email = "test@test.com"
        };
        
        var response = await Client.PostAsJsonAsync("/api/watch",  request);
        
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
    }

    [TestMethod]
    // Url
    [DataRow("Missing URL", "", 100, "test@test.com")]
    [DataRow("Invalid URL Format", "not-a-url", 100, "test@test.com")]
    // Target price
    [DataRow("Free Price", "https://amazon.co.uk/Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB", 0, "test@test.com")]
    [DataRow("Negative Price", "https://amazon.co.uk/Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB", -1, "test@test.com")]
    // Email
    [DataRow("Invalid Email", "https://amazon.co.uk/Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB", 100, "bad-email")]
    [DataRow("Missing Email", "https://amazon.co.uk/Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB", 100, "")]
    public async Task Given_Invalid_Payload_Returns_400(string scenario, string url, double price, string email)
    {
        var request = new { Url = url, TargetPrice = price, Email = email };
     
        var response = await Client.PostAsJsonAsync("/api/watch", request);
     
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, $"Failed on scenario: {scenario}");
    }
}