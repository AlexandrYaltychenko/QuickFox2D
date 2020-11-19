using System;
namespace QuickFox.Components
{
    public class UserClickableComponent : Component
    {
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int Order { get; set; }
    }
}
