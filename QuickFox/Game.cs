using System;
using QuickFox;
using QuickFox.Args;
using QuickFox.Engine;
using QuickFox.Engine.Providers;
using QuickFox.Managers;
using QuickFox.Providers;
using QuickFox.Rendering;
using QuickFox.Settings;
using QuickFox.Systems;
using QuickFox.UI;
using Unity;

namespace Skia.Motion.Engine
{
    public abstract class Game<T, S, K> where T : IRenderingProvider<S> where S : ISurface where K : IResourceProvider
    {
        protected readonly GameSettings Settings;
        protected IComponentManager ComponentManager;
        protected IResourceManager ResourceManager;
        protected ISystemManager SystemManager;
        protected ITimeManager TimeManager;
        protected IRenderingProvider<S> RenderingProvider;
        protected IResourceProvider ResourceProvider;
        protected IEntityManager EntityManager;
        protected IEventManager EventManager;
        protected IUIManager UIManager;
        protected BaseUserInputProvider UserInputProvider;
        protected Scene _currentScene;

        protected Game(GameSettings settings)
        {
            Settings = settings;
            Initialize();
        }

        public void SetSurfaceToRender(S surface)
        {
            RenderingProvider.SetSurface(surface);
        }

        protected virtual void OnStart() { }

        protected virtual void OnStop() { }

        protected virtual void OnPause() { }

        protected virtual void OnResume() { }

        private void Initialize()
        {
            Fox.Container.RegisterInstance(Settings);

            SystemManager = Fox.Container.Resolve<SystemManager>();
            ComponentManager = Fox.Container.Resolve<ComponentManager>();
            EntityManager = Fox.Container.Resolve<EntityManager>();
            EventManager = Fox.Container.Resolve<EventManager>();
            TimeManager = Fox.Container.Resolve<TimeManager>();
            ResourceProvider = Fox.Container.Resolve<K>();
            ResourceManager = new ResourceManager(ResourceProvider);
            UIManager = new UIManager(EntityManager, ComponentManager);
            Fox.Container.RegisterInstance<IUIManager>(UIManager);

            Fox.Container.RegisterInstance<IEventManager>(EventManager);
            Fox.Container.RegisterInstance<ISystemManager>(SystemManager);
            Fox.Container.RegisterInstance<IResourceManager>(ResourceManager);
            Fox.Container.RegisterInstance<IComponentManager>(ComponentManager);
            Fox.Container.RegisterInstance<IEntityManager>(EntityManager);
            Fox.Container.RegisterInstance<ITimeManager>(TimeManager);
            Fox.Container.RegisterInstance(Settings);

            UserInputProvider = Fox.Container.Resolve<BaseUserInputProvider>();

            var statsSystem = Fox.Container.Resolve<StatsSystem>();
            Fox.Container.RegisterInstance<IStatsSystem>(statsSystem);

            RenderingProvider = Fox.Container.Resolve<T>();

            Fox.Container.RegisterInstance<IRenderingProvider>(RenderingProvider);
            Fox.Container.RegisterInstance<IRenderingProvider<S>>(RenderingProvider);
            Fox.Container.RegisterInstance<IMeasurementProvider>(RenderingProvider.MeasurementProvider);

            var renderingSystem = RenderingProvider.CreateRenderingSystem();

            var movementSystem = GetMovementSystem();
            var collisionSystem = Fox.Container.Resolve<CollisionSystem>();
            var userInteractionSystem = Fox.Container.Resolve<UserInteractionSystem>();
            var countdownSystem = Fox.Container.Resolve<CountdownSystem>();
            var animationSystem = Fox.Container.Resolve<AnimationSystem>();

            SystemManager.Register(renderingSystem);
            SystemManager.Register(movementSystem);
            SystemManager.Register(collisionSystem);
            SystemManager.Register(userInteractionSystem);
            SystemManager.Register(countdownSystem);
            SystemManager.Register(animationSystem);
        }

        private void GameUpdatePublished(object sender, EventArgs e)
        {

        }

        public void Start()
        {
            UserInputProvider.Enable();
            OnStart();

            Resume();
        }

        public void Stop()
        {
            UserInputProvider.Disable();
            OnStop();
        }

        public void Resume()
        {
            OnResume();
        }

        public void Pause()
        {
            OnPause();
        }

        public void LoadScene<K>() where K : Scene
        {
            var scene = Fox.Container.Resolve<K>();

            if (_currentScene != null)
            {
                _currentScene.OnUnload();
            }

            scene.OnLoad();
            scene.OnUIPrepare();

            EventManager.Post(new SceneChangedArgs(scene));
        }

        protected virtual IMovementSystem GetMovementSystem()
        {
            return Fox.Container.Resolve<MovementSystem>();
        }
    }
}
