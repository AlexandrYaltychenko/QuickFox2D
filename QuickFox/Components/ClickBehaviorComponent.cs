using System;
using QuickFox.Managers;
using QuickFox.Rendering;

namespace QuickFox.Components
{
    public abstract class ClickBehaviorComponent : Component
    {
        protected readonly IEntityManager EntityManager;
        protected readonly IComponentManager ComponentManager;

        public ClickBehaviorComponent(IEntityManager entityManager, IComponentManager componentManager)
        {
            EntityManager = entityManager;
            ComponentManager = componentManager;
        }

        public abstract void OnClick(Point position, Scene scene);
    }
}
