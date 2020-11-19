using System;
namespace QuickFox.Components
{
    public class TerrainComponent : Component, IRenderableComponent
    {
        public string ResourceId { get; set; }
        public int Order { get; set; } = 0;
    }
}
