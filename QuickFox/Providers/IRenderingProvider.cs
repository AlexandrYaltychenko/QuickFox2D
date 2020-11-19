using System;
using QuickFox.Managers;
using QuickFox.Rendering;
using QuickFox.Systems;
using QuickFox.UI;

namespace QuickFox.Engine.Providers
{
    public interface IRenderingProvider
    {
        ISurfaceInfo SurfaceInfo { get; }

        IMeasurementProvider MeasurementProvider { get; }
    }

    public interface IRenderingProvider<T> : IRenderingProvider where T : ISurface
    {
        IRenderingSystem CreateRenderingSystem();

        void SetSurface(T surface);
    }
}
