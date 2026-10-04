using CinemaAbyss.Proxy.Application.Common.Models;

namespace CinemaAbyss.Proxy.Application.Common.Interfaces;

public interface IRequestForwarder
{
    Task<ForwardResponse> ForwardAsync(string baseUrl, ForwardRequest request, CancellationToken cancellationToken);
}
