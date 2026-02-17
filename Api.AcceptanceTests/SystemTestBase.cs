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

namespace Api.AcceptanceTests;

[TestClass]
public abstract class SystemTestBase
{
    static string dbPassword = Guid.NewGuid().ToString();
    
    protected static string? MySqlConnectionString { get; private set; }
    private static MySqlContainer mysqlContainer = new MySqlBuilder()
        .WithImage("mysql:8.0")
        .WithDatabase("WatchDb")
        .WithUsername("test_user")
        .WithPassword(dbPassword)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilInternalTcpPortIsAvailable(3306))
        .Build();
    
    protected static HttpClient Client;
    static WebApplicationFactory<Program> factory;
    
    [AssemblyInitialize]
    public static async Task AssemblyInitialize(TestContext context)
    {
        await mysqlContainer.StartAsync();
        MySqlConnectionString = mysqlContainer.GetConnectionString();
        
        factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = mysqlContainer.GetConnectionString()
                });
            });
        });

        Client = factory.CreateClient();
        
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WatchDbContext>();
        await db.Database.EnsureCreatedAsync();
    }
    
    [AssemblyCleanup]
    public static async Task AssemblyCleanup()
    {
        await mysqlContainer.DisposeAsync();
        Client.Dispose();
        await factory.DisposeAsync();
    }
}