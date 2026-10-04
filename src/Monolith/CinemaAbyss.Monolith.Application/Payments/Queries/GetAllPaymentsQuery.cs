using CinemaAbyss.Monolith.Application.Common.DTOs;
using CinemaAbyss.Monolith.Application.Common.Interfaces;
using CinemaAbyss.Monolith.Application.Common.Mappings;
using MediatR;

namespace CinemaAbyss.Monolith.Application.Payments.Queries;

public record GetAllPaymentsQuery(int? UserId) : IRequest<IReadOnlyList<PaymentDto>>;

public class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQuery, IReadOnlyList<PaymentDto>>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetAllPaymentsQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<IReadOnlyList<PaymentDto>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = request.UserId.HasValue
            ? await _paymentRepository.GetByUserIdAsync(request.UserId.Value, cancellationToken)
            : await _paymentRepository.GetAllAsync(cancellationToken);

        return payments.Select(p => p.ToDto()).ToList();
    }
}
