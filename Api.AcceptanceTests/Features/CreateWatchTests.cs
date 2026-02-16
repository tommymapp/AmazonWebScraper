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
}