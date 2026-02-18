using Api.Contexts;
using Api.DTOs;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WatchController : ControllerBase
{
    private readonly WatchDbContext watchDbContext;
    private string baseUrl;

    public WatchController(WatchDbContext watchDbContext, IConfiguration config)
    {
        this.watchDbContext = watchDbContext;
        baseUrl = config["AmazonSettings:BaseUrl"] ?? "amazon.co.uk";
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWatchRequest request)
    {
        var watch = new Watch(
            Guid.NewGuid(), 
            request.Url,
            request.TargetPrice,
            request.Email, 
            "Active",
            baseUrl
        );

        try
        {
            watchDbContext.Watches.Add(watch);
            await watchDbContext.SaveChangesAsync();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
        }
        

        var response = new CreateWatchResponse()
        {
            Id = watch.Id,
            Email = watch.Email,
            Url = watch.Url,
            TargetPrice = watch.TargetPrice,
            Status = watch.Status
        };
        
        return Created($"/api/watch/{response.Id}", response);
    }
}