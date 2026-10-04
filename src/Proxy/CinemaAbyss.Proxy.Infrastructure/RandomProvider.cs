using CinemaAbyss.Proxy.Application.Common.Interfaces;

namespace CinemaAbyss.Proxy.Infrastructure;

public class RandomProvider : IRandomProvider
{
    public int NextPercentRoll() => Random.Shared.Next(0, 100);
}
