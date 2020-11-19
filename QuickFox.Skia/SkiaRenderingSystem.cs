using System;
using System.Collections.Generic;
using System.Linq;
using QuickFox.Args;
using QuickFox.Components;
using QuickFox.Managers;
using QuickFox.Rendering;
using QuickFox.Settings;
using QuickFox.Skia.Args;
using QuickFox.Skia.Rendering;
using QuickFox.Skia.Resources;
using QuickFox.States;
using QuickFox.Systems;
using QuickFox.UI;
using SkiaSharp;

namespace QuickFox.Sia
{
    public class SkiaRenderingSystem : IRenderingSystem
    {
        private readonly IEntityManager _entityManager;
        private readonly IEventManager _eventManager;
        private readonly IComponentManager _componentManager;
        private readonly IUIManager _uiManager;
        private readonly IResourceManager _memoryManager;
        private readonly IStatsSystem _statsSystem;
        private readonly IDictionary<Type, ISkiaRenderer> _renderers;
        private readonly SKPaint _defaultPaint;
        private readonly SKPaint _sceneBorderPaint;
        private readonly SKPaint _pauseOverlayPaint;
        private readonly GameSettings _gameSettings;
        private Transformation _currentTransformation;
        private SKSurface _bufferredSurface;
        private SKSizeI _bufferSize;

        public SkiaRenderingSystem(IEntityManager entityManager,
                                   IEventManager eventManager,
                                   IUIManager uiManager,
                                   IComponentManager componentManager,
                                   IResourceManager memoryManager,
                                   IStatsSystem statsSystem,
                                   GameSettings gameSettings)
        {
            _entityManager = entityManager;
            _eventManager = eventManager;
            _uiManager = uiManager;
            _componentManager = componentManager;
            _memoryManager = memoryManager;
            _statsSystem = statsSystem;
            _gameSettings = gameSettings;

            _defaultPaint = new SKPaint
            {
                Color = SKColors.White,
                TextSize = 24,
                StrokeWidth = 2
            };

            _sceneBorderPaint = new SKPaint
            {
                Color = SKColors.Red,
                StrokeWidth = 6,
                Style = SKPaintStyle.Stroke
            };

            _pauseOverlayPaint = new SKPaint
            {
                Color = new SKColor(0, 0, 0, 100),
                Style = SKPaintStyle.Fill
            };

            _renderers = new Dictionary<Type, ISkiaRenderer>
            {
                { typeof(SpriteComponent),  new SpriteRenderer(_memoryManager)},
                { typeof(HealthComponent), new HealthRenderer() },
                { typeof(TiledSpriteComponent), new TiledSpriteRenderer(_memoryManager) },
                { typeof(AnimatedSpriteComponent), new AnimatedSpriteRenderer(_memoryManager) },
                { typeof(BackgroundComponent), new BackgroundRenderer(_memoryManager) },
                { typeof(TextComponent), new TextRenderer() }
            };

            _eventManager.Subscribe<RenderingRequiredArgs>(OnRender);
            _eventManager.Subscribe<SceneChangedArgs>(OnSceneChanged);
        }

        public Scene CurrentScene { get; set; }

        public Rect CurrentViewport => new Rect { X1 = 0, Y1 = 0, X2 = _bufferSize.Width, Y2 = _bufferSize.Height };

        public Transformation CurrentTransformation => _currentTransformation;

        private void OnSceneChanged(SceneChangedArgs args)
        {
            CurrentScene = args.Scene;
        }

        private void OnRender(RenderingRequiredArgs args)
        {
            var scene = CurrentScene;

            if (scene == null)
                return;

            _bufferSize = new SKSizeI(
                (int)(args.Viewport.Width / _gameSettings.RenderingScale),
                (int)(args.Viewport.Height / _gameSettings.RenderingScale));

            args.Surface.Canvas.Scale(_gameSettings.RenderingScale);

            var canvas = args.Surface.Canvas; // GetBufferedSurface(new SKSizeI((int)args.Viewport.Width, (int)args.Viewport.Height), _gameSettings.RenderingScale);
            canvas.DrawColor(SKColors.Black);

            var sceneViewport = RenderScene(canvas, scene.State);

            canvas.ResetMatrix();

            args.Surface.Canvas.Scale(_gameSettings.RenderingScale);

            if (scene.State != SceneState.Playing)
            {
                canvas.DrawRect(new SKRect(0, 0, _bufferSize.Width, _bufferSize.Height), _pauseOverlayPaint);
            }

            RenderUI(args.Viewport, canvas, scene.State);

            var fpsInfo = $"FPS: {_statsSystem.CurrentFPS.ToString("N0")} Resolution: {sceneViewport.Width}x{sceneViewport.Height}";
            canvas.DrawText(fpsInfo, 32, 32 + _defaultPaint.TextSize, _defaultPaint);


            //args.Surface.Canvas.DrawSurface(args.Surface, 0f, 0f);
            //args.Surface.Canvas.DrawSurface(_bufferredSurface, 0f, 0f);

            _eventManager.Post(new SceneUpdatedArgs(scene, new Rect { X1 = 0, Y1 = 0, X2 = sceneViewport.Width, Y2 = sceneViewport.Height }, new Rect { X1 = args.Viewport.Left, Y1 = args.Viewport.Top, X2 = args.Viewport.Right, Y2 = args.Viewport.Bottom }));
        }

        private SKRect RenderScene(SKCanvas canvas, SceneState sceneState)
        {
            var scene = CurrentScene;
            var camera = scene.CurrentCamera;

            camera.Update();

            var cameraX = camera.X;
            var cameraY = camera.Y;
            var cameraScale = camera.Z;

            var viewportWidth = _bufferSize.Width * cameraScale;
            var viewportHeight = _bufferSize.Height * cameraScale;
            var realViewport = new SKRect(
                cameraX - viewportWidth / 2,
                cameraY - viewportHeight / 2,
                cameraX + viewportWidth / 2,
                cameraY + viewportHeight / 2);

            var entities = _entityManager.GetAllGameEntitiesForScene<GameEntity>(scene.Id);

            canvas.Translate(-cameraX + _bufferSize.Width / cameraScale / 2, -cameraY + _bufferSize.Height / cameraScale / 2);
            canvas.Scale(cameraScale, cameraScale, cameraX - _bufferSize.Width / cameraScale / 2, cameraY - _bufferSize.Height / cameraScale / 2);

            var renderJobs = new List<Tuple<IRenderableComponent, PositionComponent, BodyComponent, AlphaComponent>>();

            foreach (var entity in entities)
            {
                var entityComponents = _componentManager.GetComponentsForEntity(entity.Id);

                var renderableComponents = entityComponents.OfType<IRenderableComponent>();

                foreach (var renderableComponent in renderableComponents)
                {
                    var body = entityComponents.OfType<BodyComponent>().FirstOrDefault();
                    var position = entityComponents.OfType<PositionComponent>().FirstOrDefault();
                    var alpha = entityComponents.OfType<AlphaComponent>().FirstOrDefault();

                    if (body == null || position == null)
                        continue;

                    renderJobs.Add(Tuple.Create(renderableComponent, position, body, alpha));
                }
            }

            var orderedRenderables = renderJobs.OrderBy(r => GetOrderFromRenderableAndPosition(r.Item1.Order, r.Item2.X, r.Item2.Y));

            foreach (var job in orderedRenderables)
            {
                var position = job.Item2;
                var body = job.Item3;
                var renderableComponent = job.Item1;

                var left = position.X - body.Width / 2;
                var top = position.Y - body.Height / 2;
                var right = left + body.Width;
                var bottom = top + body.Height;

                var location = new SKRect(left, top, right, bottom);

                if (_renderers.TryGetValue(renderableComponent.GetType(), out var renderer))
                {
                    renderer.Render(canvas, renderableComponent, location, job.Item4?.Alpha ?? 1f);
                }
            }

            return realViewport;
        }

        private void RenderUI(SKRect viewport, SKCanvas canvas, SceneState sceneState)
        {
            var scene = CurrentScene;
            var uiLayers = _uiManager.GetActiveLayersForScene(scene.Id);

            var uiRenderJobs = new List<Tuple<IRenderableComponent, SKPoint, SKSize, float>>();

            foreach (var uiLayer in uiLayers.OrderBy(l => l.Order))
            {
                if (!uiLayer.Bounds.IsValid)
                    continue;

                var elements = _uiManager.GetUIElementsForLayer(uiLayer.Id);
                var renderableElements = elements.ToList();
                renderableElements.Add(uiLayer);

                foreach (var element in renderableElements)
                {
                    var entityComponents = _componentManager.GetComponentsForEntity(element.Id);
                    var renderableComponents = entityComponents.OfType<IRenderableComponent>();
                    var position = new SKPoint(element.Bounds.X, element.Bounds.Y);
                    var size = new SKSize(element.Bounds.Width, element.Bounds.Height);

                    foreach (var renderableComponent in renderableComponents)
                    {
                        uiRenderJobs.Add(Tuple.Create(renderableComponent, position, size, GetOrderFromRenderableAndPosition(uiLayer.Order, position.X, position.Y)));
                    }
                }
            }

            var orderedRenderables = uiRenderJobs.OrderBy(r => r.Item1.Order);

            foreach (var job in orderedRenderables)
            {
                var position = job.Item2;
                var body = job.Item3;
                var renderableComponent = job.Item1;

                var left = position.X;
                var top = position.Y;
                var right = left + body.Width;
                var bottom = top + body.Height;

                var location = new SKRect(left, top, right, bottom);

                if (_renderers.TryGetValue(renderableComponent.GetType(), out var renderer))
                {
                    renderer.Render(canvas, renderableComponent, location, 1f);
                }
            }
        }

        private SKCanvas GetBufferedSurface(SKSizeI size, float scale)
        {
            var requiredBufferSize = new SKSizeI((int)(size.Width / _gameSettings.RenderingScale), (int)(size.Height / _gameSettings.RenderingScale));

            if (_bufferredSurface == null)
            {
                _bufferredSurface = CreateBufferForRect(requiredBufferSize);
            }
            else if (requiredBufferSize != _bufferSize)
            {
                _bufferredSurface.Dispose();
                _bufferredSurface = CreateBufferForRect(requiredBufferSize);
            }

            _bufferSize = requiredBufferSize;

            return _bufferredSurface.Canvas;
        }

        private SKSurface CreateBufferForRect(SKSizeI size)
        {
            var grContext = GRContext.Create(GRBackend.OpenGL);
            return SKSurface.Create(grContext, true, new SKImageInfo(size.Width, size.Height));
        }

        private float GetOrderFromRenderableAndPosition(int renderableOrder, float positionX, float positionY)
        {
            return renderableOrder * 1000 + positionY / 1000f;
        }
    }
}
