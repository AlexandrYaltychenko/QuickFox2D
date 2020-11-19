using System;
using System.Collections.Generic;
using System.Linq;
using QuickFox.Args;
using QuickFox.Collisions;
using QuickFox.Components;
using QuickFox.Engine.Providers;
using QuickFox.Managers;
using QuickFox.Settings;
using QuickFox.States;
using QuickFox.Systems;
using QuickFox.UI;

namespace QuickFox.Rendering
{
    public abstract class Scene
    {
        protected readonly IComponentManager ComponentManager;
        protected readonly IEntityManager EntityManager;
        protected readonly IUIManager UIManager;
        protected readonly IResourceManager ResourceManager;
        protected readonly IEventManager EventManager;
        protected readonly IRenderingProvider RenderingProvider;
        protected readonly GameSettings GameSettings;

        public string Id { get; }

        public float Width { get; protected set; }
        public float Height { get; protected set; }

        public SceneState State { get; protected set; }

        private Rect _currentViewport;

        public Scene(IComponentManager componentManager,
                     IEntityManager entityManager,
                     IUIManager uiManager,
                     IResourceManager resourceManager,
                     IEventManager eventManager,
                     IRenderingProvider renderingProvider,
                     GameSettings gameSettings)
        {
            ComponentManager = componentManager;
            EntityManager = entityManager;
            UIManager = uiManager;
            ResourceManager = resourceManager;
            EventManager = eventManager;
            RenderingProvider = renderingProvider;
            GameSettings = gameSettings;
            Id = Guid.NewGuid().ToString();
            EventManager.Subscribe<SceneUpdatedArgs>(OnSceneUpdated);
            EventManager.Subscribe<KeyEventArgs>(OnKeyboardAction);
            EventManager.Subscribe<PointerEventArgs>(OnPointerAction);
        }

        public ICamera CurrentCamera { get; set; }

        protected virtual void OnSceneUpdated(SceneUpdatedArgs args)
        {
            if (State == SceneState.Playing)
            {
                var entities = EntityManager.GetAllEntitiesForScene(Id);

                foreach (var entity in entities)
                {
                    var behaviorComponents = ComponentManager.GetComponentsForEntity(entity.Id).OfType<IBehaviorComponent>();
                    foreach (var behaviorComponent in behaviorComponents)
                    {
                        behaviorComponent.Update(this, entity);
                    }
                }
            }

            if (!args.ScreenViewport.Equals(_currentViewport))
            {
                _currentViewport = args.ScreenViewport;
                var rootLayers = UIManager.GetActiveLayersForScene(Id);
                foreach (var rootLayer in rootLayers)
                {
                    rootLayer.Invalidate();
                    rootLayer.RequestLayout((int)(_currentViewport.Width / GameSettings.RenderingScale), (int)(_currentViewport.Height / GameSettings.RenderingScale));
                }
            }
        }

        protected virtual void OnKeyboardAction(KeyEventArgs args)
        {

        }

        protected virtual void OnPointerAction(PointerEventArgs args)
        {

        }

        public virtual void OnLoad()
        {
        }

        public virtual void OnUIPrepare()
        {
        }

        public virtual void OnUnload()
        {
        }

        public IEnumerable<DistantiatedEntity> GetEntitiesInDistance<T>(Point p, float distance, Func<IEntity, IList<IComponent>, bool> predicate = null) where T : IEntity
        {
            var results = new List<DistantiatedEntity>();
            var entities = EntityManager.GetAllEntitiesForScene<T>(Id);
            foreach (var entity in entities)
            {
                var components = ComponentManager.GetComponentsForEntity(entity.Id);
                var body = components.OfType<BodyComponent>().FirstOrDefault();
                var position = components.OfType<PositionComponent>().FirstOrDefault();
                if (body == null || position == null)
                    continue;
                if (predicate?.Invoke(entity, components) ?? true)
                {
                    var radius = Math.Max(body.Width, body.Height);
                    var actualDistance = Math.Sqrt(Math.Pow(p.X - position.X, 2) + Math.Pow(p.Y - position.Y, 2));
                    if (actualDistance <= distance)
                    {
                        var distantiatedEntity = new DistantiatedEntity(position, body, (float)actualDistance);
                        results.Add(distantiatedEntity);
                    }
                }
            }

            return results.OrderBy(d => d.Distance);
        }

        public EntityBuilder<T> GetEntityBuilder<T>() where T : IEntity, new()
        {
            return new EntityBuilder<T>(EntityManager, ComponentManager);
        }

        protected void UpdatePositions()
        {
        }

        protected UILayer AddLayer(VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, string backgroundId = null)
        {
            var layer = UIManager.CreateRootLayer();
            layer.VerticalAlignment = verticalAlignment;
            layer.HorizontalAlignment = horizontalAlignment;
            layer.SceneId = Id;
                
            if (!string.IsNullOrWhiteSpace(backgroundId))
            {
                var backgroundComponent = ComponentManager.CreateComponent<BackgroundComponent>(layer.Id);
                backgroundComponent.BackgroundResourceId = backgroundId;
            }

            return layer;
        }

    }

    public struct DistantiatedEntity
    {
        public string EntityId { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Distance { get; set; }

        public PositionComponent Position { get; set; }

        public BodyComponent Body { get; set; }

        public DistantiatedEntity(PositionComponent position, BodyComponent body, float distance)
        {
            Position = position;
            Body = body;
            EntityId = position.EntityId;
            Width = body.Width;
            Height = body.Height;
            X = position.X;
            Y = position.Y;
            Distance = distance;
        }
    }

}
