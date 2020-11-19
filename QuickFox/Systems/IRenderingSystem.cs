using System;
using QuickFox.Rendering;

namespace QuickFox.Systems
{
    public interface IRenderingSystem : ISystem
    {
        Scene CurrentScene { get; }

        public Rect CurrentViewport { get; }

        public Transformation CurrentTransformation { get; }
    }
}
