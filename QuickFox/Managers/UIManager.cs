using System;
using System.Collections.Generic;
using System.Linq;
using QuickFox.Engine.Providers;
using QuickFox.Extensions;
using QuickFox.Rendering;
using QuickFox.UI;
using Unity;

namespace QuickFox.Managers
{
    public class UIManager : IUIManager
    {
        private readonly IComponentManager _componentManager;
        private readonly IEntityManager _entityManager;
        private readonly IDictionary<string, UILayer> _layers;

        public UIManager(IEntityManager entityManager, IComponentManager componentManager)
        {
            _entityManager = entityManager;
            _componentManager = componentManager;
            _layers = new Dictionary<string, UILayer>();
        }

        public UILayer CreateRootLayer()
        {
            var newLayer = new UILayer(this, _entityManager, _componentManager)
            {
                Order = 1
            };

            _layers.Add(newLayer.Id, newLayer);

            return newLayer;
        }

        public T CreateUIElement<T>() where T : UIEntity
        {
            var uiElement = Fox.Container.Resolve<T>();
            _entityManager.AttachEntity(uiElement);
            return uiElement;
        }

        public void DeleteLayer(string layerId)
        {
            _componentManager.DeleteComponentsOfEntity(layerId);
            _entityManager.DeleteEntity(layerId);
            GetUIElementsForLayer(layerId).ForEach(e => DeleteUIElement(e.Id));
        }

        public void DeleteUIElement(string elementId)
        {
            _componentManager.DeleteComponentsOfEntity(elementId);
            _entityManager.DeleteEntity(elementId);
        }

        public IList<UILayer> GetActiveLayersForScene(string sceneId)
        {
            return _layers.Values.Where(l => l.SceneId == sceneId && l.IsActive).ToList();
        }

        public IList<UIEntity> GetUIElementsForLayer(string layerId)
        {
            if (_layers.TryGetValue(layerId, out var layer))
            {
                var entities = _entityManager.GetAllUIEntitiesForScene<UIEntity>(layer.SceneId);
                return entities.Where(e => e.LayerId == layerId).ToList();
            }
            else return new List<UIEntity>();
        }
    }
}
