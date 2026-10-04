namespace CinemaAbyss.Proxy.Application.Common.Models;

public record ForwardRequest(
    string Method,
    string Path,
    string QueryString,
    IReadOnlyDictionary<string, string[]> Headers,
    byte[] Body,
    string? ContentType);
