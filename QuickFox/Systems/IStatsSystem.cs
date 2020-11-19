using System;
namespace QuickFox.Systems
{
    public interface IStatsSystem : ISystem
    {
        float CurrentFPS { get; }
    }
}
