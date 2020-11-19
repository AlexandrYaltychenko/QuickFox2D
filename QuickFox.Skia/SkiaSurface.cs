using System;
using QuickFox.Managers;
using QuickFox.Rendering;
using QuickFox.Skia.Args;
using SkiaSharp;

namespace QuickFox.Skia
{
    public abstract class SkiaSurface : ISurface
    {
        public float Width { get; protected set; }

        public float Height { get; protected set; }

        public abstract void Attach(IEventManager eventManager);

        public abstract void Detach();
    }
}
