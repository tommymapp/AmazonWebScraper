using Api.Contexts;
using Api.DTOs;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WatchController : ControllerBase
{
    private readonly WatchDbContext watchDbContext;

    public WatchController(WatchDbContext watchDbContext)
    {
        this.watchDbContext = watchDbContext;
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
            "amazon.co.uk"
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