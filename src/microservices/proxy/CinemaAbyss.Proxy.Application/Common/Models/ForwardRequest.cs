namespace CinemaAbyss.Proxy.Application.Common.Models;

/// <summary>A client request in a form the Application layer can route without knowing about HTTP.</summary>
public record ForwardRequest(
    string Method,
    string Path,
    string QueryString,
    IReadOnlyDictionary<string, string[]> Headers,
    byte[] Body,
    string? ContentType);
