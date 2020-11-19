using System;
using System.Linq;
using QuickFox.Managers;
using QuickFox.Rendering;

namespace QuickFox.Components
{
    public abstract class BehaviorComponent<T,S> : Component, IBehaviorComponent where T : Entity where S : Scene
    {
        protected readonly IComponentManager ComponentManager;
        protected readonly IEntityManager EntityManager;

        public BehaviorComponent(IComponentManager componentManager, IEntityManager entityManager)
        {
            ComponentManager = componentManager;
            EntityManager = entityManager;
        }

        protected K GetComponent<K>() where K : IComponent
        {
            return ComponentManager.GetComponentsForEntity(EntityId).OfType<K>().FirstOrDefault();
        }

        public void Update(Scene scene, IEntity entity)
        {
            if (entity is T typedEntity && scene is S typedScene)
            {
                Update(typedScene, typedEntity);
            }
            else throw new ArgumentException($"Cannot update entity of invalid type {entity.GetType()}");
        }

        public abstract void Update(S scene, T entity);
    }
}
