using QuickFox.Collisions;
using QuickFox.Managers;

namespace QuickFox.Components
{
    public abstract class CollisionBehaviorComponent : Component
    {
        protected readonly IEntityManager EntityManager;
        protected readonly IComponentManager ComponentManager;

        public CollisionBehaviorComponent(IEntityManager entityManager, IComponentManager componentManager)
        {
            EntityManager = entityManager;
            ComponentManager = componentManager;
        }

        public virtual void OnCollisionEnter(Collision collision)
        {
        }

        public virtual void OnCollisionStay(Collision collision)
        {
        }

        public virtual void OnCollisionExit(Collision collision)
        {
        }
    }
}
