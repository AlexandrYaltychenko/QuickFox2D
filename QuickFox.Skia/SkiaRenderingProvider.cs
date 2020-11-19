using QuickFox.Engine.Providers;
using QuickFox.Managers;
using QuickFox.Rendering;
using QuickFox.Sia;
using QuickFox.Systems;
using QuickFox.Settings;
using QuickFox.UI;

namespace QuickFox.Skia
{
    public class SkiaRenderingProvider : IRenderingProvider<SkiaSurface>
    {
        private SkiaSurface _currentSurface;
        private readonly IEntityManager _entityManager;
        private readonly IEventManager _eventManager;
        private readonly IComponentManager _componentManager;
        private readonly IUIManager _uiManager;
        private readonly IResourceManager _memoryManager;
        private readonly IStatsSystem _statsSystem;
        private readonly GameSettings _settings;

        public SkiaRenderingProvider(IEntityManager entityManager,
                                     IEventManager eventManager,
                                     IComponentManager componentManager,
                                     IUIManager uiManager,
                                     IResourceManager memoryManager,
                                     IStatsSystem statsSystem,
                                     GameSettings settings)
        {
            _entityManager = entityManager;
            _eventManager = eventManager;
            _componentManager = componentManager;
            _uiManager = uiManager;
            _memoryManager = memoryManager;
            _statsSystem = statsSystem;
            _settings = settings;
        }

        public ISurfaceInfo SurfaceInfo => GetSurfaceInfo();

        public IMeasurementProvider MeasurementProvider => new SkiaMeasurementProvider(_entityManager, _componentManager);

        public IRenderingSystem CreateRenderingSystem()
        {
            return new SkiaRenderingSystem(_entityManager, _eventManager, _uiManager, _componentManager, _memoryManager, _statsSystem, _settings);
        }

        public SkiaSurface GetCurrentSurface()
        {
            return _currentSurface;
        }

        public void SetSurface(SkiaSurface surface)
        {
            if (_currentSurface != null)
            {
                _currentSurface.Detach();
            }

            _currentSurface = surface;
            _currentSurface.Attach(_eventManager);
        }

        private SurfaceInfo GetSurfaceInfo()
        {
            if (_currentSurface != null)
            {
                return new SurfaceInfo
                {
                    Width = _currentSurface.Width,
                    Height = _currentSurface.Height
                };
            }
            else
            {
                return null;
            }
        }
    }
}
