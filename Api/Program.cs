using Api.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<WatchDbContext>((serviceProvider, options) => {
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    var connString = config.GetConnectionString("DefaultConnection");
    
    options.UseMySQL(connString!); 
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
