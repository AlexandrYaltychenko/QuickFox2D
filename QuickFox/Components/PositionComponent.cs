using System;
using QuickFox.Rendering;

namespace QuickFox.Components
{
    public class PositionComponent : Component
    {
        public float X { get; set; }

        public float Y { get; set; }

        public bool AttachedToScene { get; set; } = true;
    }
}
