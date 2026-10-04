namespace CinemaAbyss.Proxy.Application.Common.Interfaces;

public interface IRandomProvider
{
    /// <summary>Returns a value in [0, 99] used for percentage-based routing decisions.</summary>
    int NextPercentRoll();
}
