using CinemaAbyss.Proxy.Application.Common.Interfaces;
using CinemaAbyss.Proxy.Application.Common.Models;
using CinemaAbyss.Proxy.Domain.Routing;
using MediatR;

namespace CinemaAbyss.Proxy.Application.Forwarding;

public record ForwardHttpRequestCommand(ForwardRequest Request) : IRequest<ForwardResponse>;

public class ForwardHttpRequestCommandHandler : IRequestHandler<ForwardHttpRequestCommand, ForwardResponse>
{
    private readonly IMigrationPolicyProvider _policyProvider;
    private readonly IRandomProvider _randomProvider;
    private readonly IBackendUrlProvider _urlProvider;
    private readonly IRequestForwarder _forwarder;

    public ForwardHttpRequestCommandHandler(
        IMigrationPolicyProvider policyProvider,
        IRandomProvider randomProvider,
        IBackendUrlProvider urlProvider,
        IRequestForwarder forwarder)
    {
        _policyProvider = policyProvider;
        _randomProvider = randomProvider;
        _urlProvider = urlProvider;
        _forwarder = forwarder;
    }

    public Task<ForwardResponse> Handle(ForwardHttpRequestCommand request, CancellationToken cancellationToken)
    {
        var policy = _policyProvider.GetCurrentPolicy();
        var roll = _randomProvider.NextPercentRoll();

        var backend = RouteResolver.Resolve(request.Request.Path, policy, roll);
        var baseUrl = _urlProvider.GetBaseUrl(backend);

        return _forwarder.ForwardAsync(baseUrl, request.Request, cancellationToken);
    }
}
