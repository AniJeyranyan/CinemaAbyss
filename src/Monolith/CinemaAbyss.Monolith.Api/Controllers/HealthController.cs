using Microsoft.AspNetCore.Mvc;

namespace CinemaAbyss.Monolith.Api.Controllers;

[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet("/health")]
    public IActionResult Get() => Ok(new { status = true });
}
