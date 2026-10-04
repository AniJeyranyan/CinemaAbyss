namespace CinemaAbyss.Proxy.Application.Common.Models;

public record ForwardResponse(
    int StatusCode,
    IReadOnlyDictionary<string, string[]> Headers,
    byte[] Body,
    string? ContentType);
