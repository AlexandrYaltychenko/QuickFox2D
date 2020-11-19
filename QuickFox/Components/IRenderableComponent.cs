using System;
namespace QuickFox.Components
{
    public interface IRenderableComponent : IComponent
    {
        int Order { get; set; }
    }
}
