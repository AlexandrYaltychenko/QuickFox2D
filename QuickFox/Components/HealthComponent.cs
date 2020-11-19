using System;
namespace QuickFox.Components
{
    public class HealthComponent : Component, IRenderableComponent
    {
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int Order { get; set; } = 999;
    }
}
