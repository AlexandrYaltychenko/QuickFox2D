using System;
using System.Diagnostics;
using System.Linq;
using QuickFox.Args;
using QuickFox.Components;
using QuickFox.Managers;

namespace QuickFox.Systems
{
    public class AnimationSystem : IAnimationSystem
    {
        private readonly IComponentManager _componentManager;
        private readonly IEntityManager _entityManager;
        private readonly IEventManager _eventManager;
        private readonly Stopwatch _sceneWatch;

        public IEntityManager EntityManager => _entityManager;

        public AnimationSystem(IComponentManager componentManager,
                               IEntityManager entityManager,
                               IEventManager eventManager)
        {
            _componentManager = componentManager;
            _entityManager = entityManager;
            _eventManager = eventManager;
            _sceneWatch = new Stopwatch();

            _eventManager.Subscribe<SceneUpdatedArgs>(OnSceneUpdated);
            _eventManager.Subscribe<SceneChangedArgs>(OnSceneChanged);
        }

        private void OnSceneUpdated(SceneUpdatedArgs args)
        {
            _sceneWatch.Stop();

            if (args.Scene.State != States.SceneState.Playing)
            {
                return;
            }

            var animatedComponents = _componentManager.GetComponents<IAnimatedComponent>();

            foreach (var animatedComponent in animatedComponents.Where(a => a.Animated))
            {
                var entity = _entityManager.GetEntity(animatedComponent.EntityId);
                if (entity?.SceneId != args.Scene.Id)
                    continue;

                animatedComponent.RemainingDelay -= (int)_sceneWatch.ElapsedMilliseconds;

                if (animatedComponent.RemainingDelay <= 0)
                {
                    animatedComponent.RemainingDelay = animatedComponent.FrameDelay;
                    if (animatedComponent.IsRepeating)
                    {
                        animatedComponent.Current = (animatedComponent.Current + 1) % animatedComponent.FrameCount;
                    } else
                    {
                        animatedComponent.Current = Math.Min(animatedComponent.Current + 1, animatedComponent.FrameCount - 1);
                    }
                }
            }

            _sceneWatch.Restart();

        }

        private void OnSceneChanged(SceneChangedArgs args)
        {
            _sceneWatch.Reset();
        }
    }
}
