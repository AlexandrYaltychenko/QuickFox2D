using System.Collections.Generic;
using QuickFox.Rendering;
using QuickFox.UI;

namespace QuickFox.Managers
{
    public interface IUIManager
    {
        UILayer CreateRootLayer();

        void DeleteLayer(string layerId);

        T CreateUIElement<T>() where T : UIEntity;

        void DeleteUIElement(string elementId);

        IList<UILayer> GetActiveLayersForScene(string sceneId);

        IList<UIEntity> GetUIElementsForLayer(string layerId);
    }
}
