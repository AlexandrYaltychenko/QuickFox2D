using System;
using QuickFox.Rendering;

namespace QuickFox.Components
{
    public interface IBehaviorComponent : IComponent
    {
        void Update(Scene scene, IEntity entity);
    }
}
