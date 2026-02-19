using Api;
using Api.Contexts;
using Api.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<WatchDbContext>((serviceProvider, options) => {
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    var connString = config.GetConnectionString("DefaultConnection");
    
    options.UseMySQL(connString!); 
});

builder.Services.AddScoped<IWatchRepo, WatchDbContext>();
builder.Services.AddScoped<IAmazonPriceParser, AmazonPriceParser>();
builder.Services.AddHttpClient<IAmazonWebClient, AmazonWebClient>((serviceProvider, client) =>
{
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(config["AmazonSettings:BaseUrl"]!);
});
builder.Services.AddScoped<IAmazonWebScraper, AmazonWebScraper>();


var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
