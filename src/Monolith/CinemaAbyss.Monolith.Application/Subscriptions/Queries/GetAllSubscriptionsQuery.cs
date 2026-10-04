using CinemaAbyss.Monolith.Application.Common.DTOs;
using CinemaAbyss.Monolith.Application.Common.Interfaces;
using CinemaAbyss.Monolith.Application.Common.Mappings;
using MediatR;

namespace CinemaAbyss.Monolith.Application.Subscriptions.Queries;

public record GetAllSubscriptionsQuery(int? UserId) : IRequest<IReadOnlyList<SubscriptionDto>>;

public class GetAllSubscriptionsQueryHandler : IRequestHandler<GetAllSubscriptionsQuery, IReadOnlyList<SubscriptionDto>>
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public GetAllSubscriptionsQueryHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<IReadOnlyList<SubscriptionDto>> Handle(GetAllSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var subscriptions = request.UserId.HasValue
            ? await _subscriptionRepository.GetByUserIdAsync(request.UserId.Value, cancellationToken)
            : await _subscriptionRepository.GetAllAsync(cancellationToken);

        return subscriptions.Select(s => s.ToDto()).ToList();
    }
}
