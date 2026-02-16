using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Api.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlConnector;

namespace Api.AcceptanceTests.Features;

[TestClass]
public class CreateWatchTests : BaseAcceptanceTest
{
    [TestMethod]
    public async Task Given_ValidRequest_Then_ReturnsStatus201()
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
    public async Task Given_InvalidPayload_Then_ReturnsStatus400(string scenario, string url, double price, string email)
    {
        var request = new { Url = url, TargetPrice = price, Email = email };
     
        var response = await Client.PostAsJsonAsync("/api/watch", request);
     
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, $"Failed on scenario: {scenario}");
    }

    [TestMethod]
    public async Task Given_ValidRequest_Then_DataIsInsertedIntoMySQLDB()
    {
        var request = new
        {
            Url = "https://amazon.co.uk/Keychron-K2-HE-Wireless-Mechanical/dp/B0F63BK4ZB",
            TargetPrice = 100,
            Email = "test@test.com"
        };
        
        var response = await Client.PostAsJsonAsync("/api/watch",  request);
        var createdWatchResponse = await response.Content.ReadFromJsonAsync<CreateWatchResponse>();
        var id = createdWatchResponse?.Id;

        Assert.IsNotNull(id, "The API should have returned an Id");
        await using var connection = new MySqlConnection(MySqlConnectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT Url, TargetPrice, Email, Status FROM watches WHERE Id = @id";
        command.Parameters.AddWithValue("@id", id);

        await using var reader = await command.ExecuteReaderAsync();
    
        if (await reader.ReadAsync())
        {
            Assert.AreEqual(request.Url, reader.GetString("Url"));
            Assert.AreEqual(request.TargetPrice, reader.GetDecimal("TargetPrice"));
            Assert.AreEqual(request.Email, reader.GetString("Email"));
            Assert.AreEqual("Active", reader.GetString("Status"));
        }
        else
        {
            Assert.Fail("No record found in database for the given ID.");
        }
    }
}