using System;
using SkiaSharp;

namespace QuickFox.Skia.Args
{
    public class RenderingRequiredArgs : EventArgs
    {
        public RenderingRequiredArgs(SKSurface surface, SKRect viewport)
        {
            Surface = surface;
            Viewport = viewport;
        }

        public SKSurface Surface { get; }
        public SKRect Viewport { get; }

    }
}
