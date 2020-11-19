using System;
using QuickFox.Rendering;

namespace QuickFox.Components
{
    public class SpriteComponent : Component, IRenderableComponent
    {
        public string ResourceId { get; set; }

        public int Order { get; set; } = 1;
    }
}
