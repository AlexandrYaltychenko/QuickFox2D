using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using QuickFox.Args;
using QuickFox.Components;
using QuickFox.Managers;
using QuickFox.Rendering;

namespace QuickFox.Systems
{
    public class MovementSystem : IMovementSystem
    {
        protected IEventManager EventManager { get; }
        protected IComponentManager ComponentManager { get; }
        protected IEntityManager EntityManager { get; }

        public MovementSystem(IEventManager eventManager,
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

            foreach (var entity in entities)
            {
                var entityComponents = ComponentManager.GetComponentsForEntity(entity.Id);
                var position = entityComponents.OfType<PositionComponent>().FirstOrDefault();
                if (position == null)
                    continue;


                var speed = entityComponents.OfType<SpeedComponent>().FirstOrDefault();
                var path = entityComponents.OfType<PathMovementComponent>().FirstOrDefault();
                var catcher = entityComponents.OfType<CatchTargetComponent>().FirstOrDefault();

                float realXSpeed = 0f;
                float realYSpeed = 0f;

                if (speed != null)
                {
                    realXSpeed = speed.XSpeed;
                    realYSpeed = speed.YSpeed;
                }
                else if (path != null)
                {
                    var currentTarget = path.CurrentTarget;
                    if (currentTarget.HasValue)
                    {
                        var diffX = Math.Abs(currentTarget.Value.Y - position.Y);
                        var diffY = Math.Abs(currentTarget.Value.X - position.X);

                        if (diffX > path.Speed || diffY > path.Speed)
                        {
                            realYSpeed = (position.Y > currentTarget.Value.Y ? -1 : 1) * diffX / (diffX + diffY) * path.Speed;
                            realXSpeed = (position.X > currentTarget.Value.X ? -1 : 1) * diffY / (diffX + diffY) * path.Speed;
                        }

                        if (Math.Abs(realXSpeed) < 0.1f && Math.Abs(realYSpeed) < 0.1f)
                        {
                            path.ChooseNext();
                        }
                    }
                }
                else if (catcher != null)
                {
                    var targetPosition = ComponentManager.GetComponent<PositionComponent>(catcher.TargetPositionId);

                    if (targetPosition != null)
                    {
                        var diffX = Math.Abs(targetPosition.Y - position.Y);
                        var diffY = Math.Abs(targetPosition.X - position.X);

                        if (diffX > catcher.Speed || diffY > catcher.Speed)
                        {
                            realYSpeed = (position.Y > targetPosition.Y ? -1 : 1) * diffX / (diffX + diffY) * catcher.Speed;
                            realXSpeed = (position.X > targetPosition.X ? -1 : 1) * diffY / (diffX + diffY) * catcher.Speed;
                        }
                    } else //TODO handle target destruction properly
                    {
                        EntityManager.DeleteEntity(entity.Id);
                        ComponentManager.DeleteComponentsOfEntity(entity.Id);
                    }
                }

                position.X += realXSpeed;
                position.Y += realYSpeed;
            }
        }

    }
}
