using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testcontainers.MySql;

namespace Api.AcceptanceTests;

[TestClass]
public class BaseAcceptanceTest : IAsyncDisposable
{
    protected string? MySqlConnectionString { get; }
    private static MySqlContainer mysqlContainer = new MySqlBuilder()
        .WithImage("mysql:8.0")
        .WithDatabase("WatchDb")
        .WithUsername("root")
        .WithPassword("password123")
        .Build();
    
    protected readonly HttpClient Client;
    readonly WebApplicationFactory<Program> factory;
    
    [AssemblyInitialize]
    public static async Task AssemblyInitialize(TestContext context)
    {
        await mysqlContainer.StartAsync();
    }
    
    protected BaseAcceptanceTest()
    {
        MySqlConnectionString = mysqlContainer.GetConnectionString();

        factory = new WebApplicationFactory<Program>();
        Client = factory.CreateClient();
    }

    public async ValueTask DisposeAsync()
    {
        await mysqlContainer.DisposeAsync();
        Client.Dispose();
        await factory.DisposeAsync();
    }
}