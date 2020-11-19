using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using QuickFox.Args;
using QuickFox.Collisions;
using QuickFox.Components;
using QuickFox.Managers;
using QuickFox.Rendering;

namespace QuickFox.Systems
{
    public class CollisionSystem : ICollisionSystem
    {
        protected IEventManager EventManager { get; }
        protected IComponentManager ComponentManager { get; }
        protected IEntityManager EntityManager { get; }

        public CollisionSystem(IEventManager eventManager,
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

            var entities = EntityManager.GetAllEntitiesForScene(args.Scene.Id);
            var collidableEntities = new List<CollisionParams>();

            foreach (var entity in entities)
            {
                var entityComponents = ComponentManager.GetComponentsForEntity(entity.Id);
                var position = entityComponents.OfType<PositionComponent>().FirstOrDefault();
                var collider = entityComponents.OfType<ColliderBoxComponent>().FirstOrDefault();
                var collisionHandler = entityComponents.OfType<CollisionBehaviorComponent>().FirstOrDefault();
                var activeCollisions = entityComponents.OfType<ActiveCollisionComponent>().ToList();

                if (collider == null || position == null)
                    continue;

                var collisionParams = new CollisionParams
                {
                    CollisionHandler = collisionHandler,
                    ActiveCollisions = activeCollisions,
                    Entity = entity,
                    Area = new RectangleF(position.X - collider.Width / 2, position.Y - collider.Height / 2, collider.Width, collider.Height)
                };

                var collisionedEntityIds = activeCollisions.Select(x => x.EntityId).ToHashSet();

                foreach (var collidableEntity in collidableEntities)
                {
                    var hasActiveCollision = collisionedEntityIds.Contains(collidableEntity.Entity.Id);

                    if (collidableEntity.Area.IntersectsWith(collisionParams.Area))
                    {
                        if (collisionHandler != null)
                        {
                            var collision = new Collision(entity, collidableEntity.Entity, collider, collidableEntity.Collider, collidableEntity.Area.ToRect());
                            collisionHandler.OnCollisionEnter(collision);
                        }

                        if (collidableEntity.CollisionHandler != null)
                        {
                            var collision = new Collision(collidableEntity.Entity, entity, collidableEntity.Collider, collider, collidableEntity.Area.ToRect());
                            collidableEntity.CollisionHandler.OnCollisionEnter(collision);
                        }
                    }
                    else if (hasActiveCollision)
                    {
                        var currentActiveCollision = collisionParams.ActiveCollisions.FirstOrDefault(x => x.TargetEntityId == collidableEntity.Entity.Id);
                        if (currentActiveCollision != null)
                        {
                            if (collisionHandler != null)
                            {
                                var collision = new Collision(entity, collidableEntity.Entity, collider, collidableEntity.Collider, collidableEntity.Area.ToRect());
                                collisionHandler.OnCollisionExit(collision);
                            }
                            ComponentManager.Delete(currentActiveCollision.Id);
                        }


                        var targetActiveCollision = collidableEntity.ActiveCollisions.FirstOrDefault(x => x.TargetEntityId == entity.Id);
                        if (targetActiveCollision != null)
                        {
                            if (collidableEntity.CollisionHandler != null)
                            {
                                var collision = new Collision(collidableEntity.Entity, entity, collidableEntity.Collider, collider, collidableEntity.Area.ToRect());
                                collidableEntity.CollisionHandler.OnCollisionExit(collision);
                            }
                            ComponentManager.Delete(targetActiveCollision.Id);
                        }
                    }
                }

                collidableEntities.Add(collisionParams);
            }
        }

        public struct CollisionParams
        {
            public IEntity Entity { get; set; }
            public ColliderBoxComponent Collider { get; set; }
            public CollisionBehaviorComponent CollisionHandler { get; set; }
            public RectangleF Area { get; set; }
            public IList<ActiveCollisionComponent> ActiveCollisions { get; set; }
        }

    }

}
