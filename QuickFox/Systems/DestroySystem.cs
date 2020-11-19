using System;
using QuickFox.Args;
using QuickFox.Components;
using QuickFox.Managers;

namespace QuickFox.Systems
{
    public class DestroySystem : IDestroySystem
    {
        protected IEventManager EventManager { get; }
        protected IComponentManager ComponentManager { get; }
        protected IEntityManager EntityManager { get; }

        public DestroySystem(IEventManager eventManager,
                              IEntityManager entityManager,
                              IComponentManager componentManager)
        {
            EventManager = eventManager;
            EntityManager = entityManager;
            ComponentManager = componentManager;

            EventManager.Subscribe<SceneUpdatedArgs>(OnGameUpdated);
        }

        protected virtual void OnGameUpdated(SceneUpdatedArgs args)
        {
            if (args.Scene.State != States.SceneState.Playing)
                return;
        }
    }
}
