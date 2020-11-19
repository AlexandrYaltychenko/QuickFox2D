using System;
using QuickFox.Components;
using QuickFox.States;
using SkiaSharp;

namespace QuickFox.Skia.Rendering
{
    public interface ISkiaRenderer
    {
        void Render(SKCanvas target, IRenderableComponent component, SKRect location, float alpha);
    }

    public interface ISkiaRenderer<T> : ISkiaRenderer where T : IRenderableComponent
    {
        void Render(SKCanvas target, T component, SKRect location, float alpha);
    }
}
