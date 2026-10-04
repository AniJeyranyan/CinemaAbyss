using CinemaAbyss.Monolith.Application.Common.DTOs;
using CinemaAbyss.Monolith.Application.Common.Interfaces;
using CinemaAbyss.Monolith.Application.Common.Mappings;
using MediatR;

namespace CinemaAbyss.Monolith.Application.Payments.Queries;

public record GetPaymentByIdQuery(int Id) : IRequest<PaymentDto?>;

public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDto?>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<PaymentDto?> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.Id, cancellationToken);
        return payment?.ToDto();
    }
}
