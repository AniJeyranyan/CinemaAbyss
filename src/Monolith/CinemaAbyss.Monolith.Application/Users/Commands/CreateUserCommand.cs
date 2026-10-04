using CinemaAbyss.Monolith.Application.Common.DTOs;
using CinemaAbyss.Monolith.Application.Common.Interfaces;
using CinemaAbyss.Monolith.Application.Common.Mappings;
using CinemaAbyss.Monolith.Domain.Entities;
using MediatR;

namespace CinemaAbyss.Monolith.Application.Users.Commands;

public record CreateUserCommand(string Username, string Email) : IRequest<UserDto>;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(request.Username, request.Email);
        var created = await _userRepository.AddAsync(user, cancellationToken);
        return created.ToDto();
    }
}
