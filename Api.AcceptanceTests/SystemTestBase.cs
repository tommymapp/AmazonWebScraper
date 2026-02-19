using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testcontainers.MySql;
using Microsoft.EntityFrameworkCore;
using Api.Contexts;
using DotNet.Testcontainers.Builders;
using Microsoft.Extensions.DependencyInjection;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Api.AcceptanceTests;

[TestClass]
public abstract class SystemTestBase
{
    static string _dbPassword = Guid.NewGuid().ToString();
    
    protected static string? MySqlConnectionString { get; private set; }
    private static readonly MySqlContainer _mysqlContainer = new MySqlBuilder()
        .WithImage("mysql:8.0")
        .WithDatabase("WatchDb")
        .WithUsername("test_user")
        .WithPassword(_dbPassword)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilInternalTcpPortIsAvailable(3306))
        .Build();
    
    protected static HttpClient? Client;
    protected static WebApplicationFactory<Program> Factory { get; private set; }
    
    protected static string? MockedAmazonUrl { get; private set; }
    protected static WireMockServer? WireMockServer { get; private set; }
    
    [AssemblyInitialize]
    public static async Task AssemblyInitialize(TestContext context)
    {   
        SetupWireMockServer();
        
        await _mysqlContainer.StartAsync();
        MySqlConnectionString = _mysqlContainer.GetConnectionString();
        
        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = _mysqlContainer.GetConnectionString(),
                    ["AmazonSettings:BaseUrl"] = MockedAmazonUrl
                });
            });
        });

        Client = Factory.CreateClient();
        
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WatchDbContext>();
        await db.Database.EnsureCreatedAsync();
    }

    private static void SetupWireMockServer()
    {
        WireMockServer = WireMockServer.Start();
        MockedAmazonUrl = WireMockServer.Url!;
        
        // Setup fake pages from TestData
        var testDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData");
        var files = Directory.GetFiles(testDataPath, "*.html");

        foreach (var filePath in files)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var htmlContent = File.ReadAllText(filePath);

            WireMockServer
                .Given(Request.Create()
                    .WithPath($"/dp/{fileName}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "text/html")
                    .WithBody(htmlContent));
        }

    }
    
    [AssemblyCleanup]
    public static async Task AssemblyCleanup()
    {
        await _mysqlContainer.DisposeAsync();
        Client?.Dispose();
        await Factory.DisposeAsync();
        WireMockServer?.Dispose();
    }
}