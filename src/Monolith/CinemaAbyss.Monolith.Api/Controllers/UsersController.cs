using CinemaAbyss.Monolith.Application.Common.DTOs;
using CinemaAbyss.Monolith.Application.Users.Commands;
using CinemaAbyss.Monolith.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAbyss.Monolith.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAll([FromQuery] int? id, CancellationToken cancellationToken)
    {
        if (id.HasValue)
        {
            var user = await _sender.Send(new GetUserByIdQuery(id.Value), cancellationToken);
            return user is null ? NotFound() : Ok(user);
        }

        var users = await _sender.Send(new GetAllUsersQuery(), cancellationToken);
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _sender.Send(new CreateUserCommand(request.Username, request.Email), cancellationToken);
        return StatusCode(StatusCodes.Status201Created, user);
    }
}

public record CreateUserRequest(string Username, string Email);
