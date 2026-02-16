using Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WatchController : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] CreateWatchRequest request)
    {
        return Created($"/api/watch/{Guid.NewGuid()}", request);
    }
}