using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.AcceptanceTests;

public class BaseAcceptanceTest : IDisposable
{
    protected readonly HttpClient Client;
    protected readonly WebApplicationFactory<Program> Factory;

    protected BaseAcceptanceTest()
    {
        Factory = new WebApplicationFactory<Program>();
        Client = Factory.CreateClient();
    }

    public void Dispose()
    {
        Client.Dispose();
        Factory.Dispose();
    }
}