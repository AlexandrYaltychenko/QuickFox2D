using System;
namespace QuickFox.Components
{
    public class TiledSpriteComponent : Component, IRenderableComponent
    {
        public string ResourceId { get; set; }
        public int Index { get; set; } = 0;
        public int Order { get; set; } = 1;
    }
}
