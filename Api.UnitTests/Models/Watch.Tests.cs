using Api.Models;

namespace Api.UnitTests.Models;

[TestClass]
public class Watch_Tests
{
    [TestMethod]
    public void When_Constructed_Then_ValuesAreCorrect()
    {
        var id = Guid.NewGuid();
        var url = "https://amazon.co.uk/Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB";
        var targetPrice = 100;
        var email = "test@test.com";
        var status = "Active";
        
        var watch = new Watch(
            id,
            url,
            targetPrice,
            email, 
            status,
            "https://amazon.co.uk/"
        );
        
        Assert.AreEqual(id, watch.Id);
        Assert.AreEqual("Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB", watch.Url);
        Assert.AreEqual(targetPrice, watch.TargetPrice);
        Assert.AreEqual(email, watch.Email);
        Assert.AreEqual(status, watch.Status);
    }

    [TestMethod]
    public void When_Constructed_Then_BaseUrlIsRemoved()
    {
        var watch = new Watch(
            Guid.NewGuid(),
            "https://amazon.co.uk/Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB",
            100,
            "test@test.com", 
            "Active",
            "https://amazon.co.uk/"
        );
        
        Assert.AreEqual("Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB", watch.Url);
        
        // Triangulation
        watch = new Watch(
            Guid.NewGuid(),
            "https://localhost:8080/Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB",
            100,
            "test@test.com", 
            "Active",
            "https://localhost:8080/"
        );
        
        Assert.AreEqual("Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB", watch.Url);
    }

    [TestMethod]
    public void When_Constructed_Then_UrlProtocolIsStripped()
    {
        var watch = new Watch(
            Guid.NewGuid(),
            "http://amazon.co.uk/Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB",
            100,
            "test@test.com", 
            "Active",
            "amazon.co.uk/"
        );
        
        Assert.AreEqual("Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB", watch.Url);
        
        // Triangulation
        watch = new Watch(
            Guid.NewGuid(),
            "https://amazon.co.uk/Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB",
            100,
            "test@test.com", 
            "Active",
            "amazon.co.uk/"
        );
        
        Assert.AreEqual("Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB", watch.Url);
    }

    [TestMethod]
    public void When_Constructed_Given_BaseUrlIsNotInUrl_Then_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => 
            new Watch(
                Guid.NewGuid(),
                "http://amazon.co.uk/Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB",
                100,
                "test@test.com", 
                "Active",
                "test.co.uk/"
            )
        );
    }
    
    [TestMethod]
    public void When_Constructed_Then_TrailingSlashIsStripped()
    {
        var watch = new Watch(
            Guid.NewGuid(),
            "http://amazon.co.uk/Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB",
            100,
            "test@test.com", 
            "Active",
            "amazon.co.uk"
        );
        
        Assert.AreEqual("Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB", watch.Url);
    }
}