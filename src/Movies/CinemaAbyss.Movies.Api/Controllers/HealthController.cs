using Microsoft.AspNetCore.Mvc;

namespace CinemaAbyss.Movies.Api.Controllers;

[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet("/api/movies/health")]
    public IActionResult Get() => Ok(new { status = true });
}
