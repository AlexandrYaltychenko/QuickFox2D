using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using QuickFox.Args;
using QuickFox.Components;
using QuickFox.Engine.Providers;
using QuickFox.Managers;
using QuickFox.Rendering;
using QuickFox.Settings;
using QuickFox.UI;

namespace QuickFox.Systems
{
    public class UserInteractionSystem : IUserInteractionSystem
    {
        private readonly Queue<UserInteractionArgs> _userInteractionQueue;
        private readonly IEntityManager _entityManager;
        private readonly IEventManager _eventManager;
        private readonly IComponentManager _componentManager;
        private readonly IRenderingProvider _renderingProvider;
        private readonly GameSettings _gameSettings;
        private Scene _currentScene;

        public UserInteractionSystem(IEntityManager entityManager,
            IEventManager eventManager,
            IComponentManager componentManager,
            IRenderingProvider renderingProvider,
            GameSettings gameSettings)
        {
            _entityManager = entityManager;
            _eventManager = eventManager;
            _componentManager = componentManager;
            _renderingProvider = renderingProvider;
            _gameSettings = gameSettings;

            _userInteractionQueue = new Queue<UserInteractionArgs>();

            _eventManager.Subscribe<PointerEventArgs>(OnUserInput);
            _eventManager.Subscribe<SceneUpdatedArgs>(OnSceneUpdated);
            _eventManager.Subscribe<SceneChangedArgs>(OnSceneChanged);
        }

        private void OnUserInput(PointerEventArgs args)
        {
            var userInteractionArgs = new UserInteractionArgs
            {
                InteractionType = InteractionType.Click,
                Args = args
            };

            _userInteractionQueue.Enqueue(userInteractionArgs);
        }

        private PointF GetSceneFromAbsCoordinates(float absX, float absY)
        {
            var sceneSize = _renderingProvider.SurfaceInfo;
            var cameraXOffset = sceneSize.Width / _gameSettings.RenderingScale / _currentScene.CurrentCamera.Z / 2 - _currentScene.CurrentCamera.X;
            var cameraYOffset = sceneSize.Height / _gameSettings.RenderingScale / _currentScene.CurrentCamera.Z / 2 - _currentScene.CurrentCamera.Y;

            var notScaledX = (absX / _gameSettings.RenderingScale / _currentScene.CurrentCamera.Z - cameraXOffset);
            var notScaledY = (absY / _gameSettings.RenderingScale / _currentScene.CurrentCamera.Z - cameraYOffset);

            var realX = notScaledX;
            var realY = notScaledY;

            return new PointF(realX, realY);
        }

        private void OnSceneChanged(SceneChangedArgs args)
        {
            _currentScene = args.Scene;
        }

        private void OnSceneUpdated(SceneUpdatedArgs args)
        {
            var sceneId = args.Scene.Id;

            if (_userInteractionQueue.Any())
            {
                ProcessUI(sceneId, _userInteractionQueue.ToList());
                ProcessScene(sceneId, _userInteractionQueue.ToList());

                _userInteractionQueue.Clear();
            }
        }

        private void ProcessScene(string sceneId, List<UserInteractionArgs> interactionsToProcess)
        {
            var entities = _entityManager.GetAllGameEntitiesForScene<GameEntity>(sceneId);

            var clickPoints = interactionsToProcess
                .Where(x => x.Handled == false)
                .Select(x => x.Args as PointerEventArgs)
                .Where(p => p.ActionType == PointerActionType.Down)
                .Select(p => GetSceneFromAbsCoordinates(p.X, p.Y))
                .ToList();

            var clickTargets = new List<ClickTarget>();

            foreach (var entity in entities)
            {
                var components = _componentManager.GetComponentsForEntity(entity.Id);
                var clickableComponent = components.OfType<UserClickableComponent>().FirstOrDefault();
                var position = components.OfType<PositionComponent>().FirstOrDefault();
                var body = components.OfType<BodyComponent>().FirstOrDefault();
                var clickHandler = components.OfType<ClickBehaviorComponent>().FirstOrDefault();

                if (clickableComponent != null && position != null && clickHandler != null)
                {
                    var width = clickableComponent.Width;
                    var height = clickableComponent.Height;

                    if (Math.Abs(width) < 0.01f && body != null)
                    {
                        width = body.Width;
                    }

                    if (Math.Abs(height) < 0.01f && body != null)
                    {
                        height = body.Height;
                    }

                    var clickableRect = new RectangleF(new PointF(position.X + clickableComponent.OffsetX - width / 2, position.Y + clickableComponent.OffsetY - height / 2), new SizeF(width, height));

                    clickTargets.Add(new ClickTarget
                    {
                        ClickableArea = clickableRect,
                        ClickBehaviorComponent = clickHandler,
                        Order = clickableComponent.Order
                    });

                }
            }

            foreach (var target in clickTargets.OrderByDescending(c => c.Order))
            {
                foreach (var p in clickPoints)
                {
                    if (target.ClickableArea.Contains(p))
                    {
                        clickPoints.Remove(p);
                        target.ClickBehaviorComponent.OnClick(new Rendering.Point { X = p.X, Y = p.Y }, _currentScene);
                        break;
                    }
                }
            }
        }

        private void ProcessUI(string sceneId, List<UserInteractionArgs> interactionsToProcess)
        {
            var entities = _entityManager.GetAllUIEntitiesForScene<UIEntity>(sceneId);

            var clickPoints = interactionsToProcess
                .Where(x => x.Handled == false)
                .Where(x => x.Args is PointerEventArgs)
                .Where(p => (p.Args as PointerEventArgs).ActionType == PointerActionType.Down)
                .Select(p => new Tuple<PointF, UserInteractionArgs>(
                    new PointF(
                        (p.Args as PointerEventArgs).X / _gameSettings.RenderingScale,
                        (p.Args as PointerEventArgs).Y / _gameSettings.RenderingScale) , p))
                .ToList();

            var clickTargets = new List<ClickTarget>();

            foreach (var entity in entities)
            {
                var components = _componentManager.GetComponentsForEntity(entity.Id);
                var clickableComponent = components.OfType<UserClickableComponent>().FirstOrDefault();
                var clickHandler = components.OfType<ClickBehaviorComponent>().FirstOrDefault();

                if (clickableComponent != null)
                {
                    var width = clickableComponent.Width;
                    var height = clickableComponent.Height;

                    if (Math.Abs(width) < 0.01f)
                    {
                        width = entity.Bounds.Width;
                    }

                    if (Math.Abs(height) < 0.01f)
                    {
                        height = entity.Bounds.Height;
                    }

                    var clickableRect = new RectangleF(
                        new PointF(
                            entity.Bounds.X + clickableComponent.OffsetX,
                            entity.Bounds.Y + clickableComponent.OffsetY),
                        new SizeF(width, height));

                    clickTargets.Add(new ClickTarget
                    {
                        ClickableArea = clickableRect,
                        ClickBehaviorComponent = clickHandler,
                        Order = clickableComponent.Order,
                    });
                }
            }

            foreach (var target in clickTargets.OrderByDescending(c => c.Order))
            {
                foreach (var p in clickPoints)
                {
                    if (target.ClickableArea.Contains(p.Item1))
                    {
                        clickPoints.Remove(p);
                        p.Item2.Handled = true;
                        target.ClickBehaviorComponent.OnClick(new Rendering.Point { X = p.Item1.X, Y = p.Item1.Y }, _currentScene);
                        break;
                    }
                }
            }
        }

        private class ClickTarget
        {
            public ClickBehaviorComponent ClickBehaviorComponent { get; set; }
            public RectangleF ClickableArea { get; set; }
            public int Order { get; set; }
        }
    }

    internal class UserInteractionArgs
    {
        public InteractionType InteractionType { get; set; }

        public EventArgs Args { get; set; }

        public bool Handled { get; set; }
    }

    internal enum InteractionType
    {
        Click
    }
}
