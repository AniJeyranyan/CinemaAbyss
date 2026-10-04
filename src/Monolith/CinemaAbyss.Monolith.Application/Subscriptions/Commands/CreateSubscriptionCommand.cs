using CinemaAbyss.Monolith.Application.Common.DTOs;
using CinemaAbyss.Monolith.Application.Common.Interfaces;
using CinemaAbyss.Monolith.Application.Common.Mappings;
using CinemaAbyss.Monolith.Domain.Entities;
using MediatR;

namespace CinemaAbyss.Monolith.Application.Subscriptions.Commands;

public record CreateSubscriptionCommand(int UserId, string PlanType, DateTime StartDate, DateTime EndDate) : IRequest<SubscriptionDto>;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, SubscriptionDto>
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public CreateSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<SubscriptionDto> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = Subscription.Create(request.UserId, request.PlanType, request.StartDate, request.EndDate);
        var created = await _subscriptionRepository.AddAsync(subscription, cancellationToken);
        return created.ToDto();
    }
}
