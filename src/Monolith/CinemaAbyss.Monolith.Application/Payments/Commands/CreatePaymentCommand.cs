using CinemaAbyss.Monolith.Application.Common.DTOs;
using CinemaAbyss.Monolith.Application.Common.Interfaces;
using CinemaAbyss.Monolith.Application.Common.Mappings;
using CinemaAbyss.Monolith.Domain.Entities;
using MediatR;

namespace CinemaAbyss.Monolith.Application.Payments.Commands;

public record CreatePaymentCommand(int UserId, decimal Amount) : IRequest<PaymentDto>;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
{
    private readonly IPaymentRepository _paymentRepository;

    public CreatePaymentCommandHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = Payment.Create(request.UserId, request.Amount);
        var created = await _paymentRepository.AddAsync(payment, cancellationToken);
        return created.ToDto();
    }
}
