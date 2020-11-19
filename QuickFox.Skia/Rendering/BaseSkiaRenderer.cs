using System;
using QuickFox.Components;
using QuickFox.States;
using SkiaSharp;

namespace QuickFox.Skia.Rendering
{
    public abstract class BaseSkiaRenderer<T> : ISkiaRenderer<T> where T : IRenderableComponent
    {
        public abstract void Render(SKCanvas target, T component, SKRect location, float alpha);

        public void Render(SKCanvas target, IRenderableComponent component, SKRect location, float alpha)
        {
            if (component is T typedComponent)
            {
                Render(target, typedComponent, location, alpha);
            }
        }
    }
}
