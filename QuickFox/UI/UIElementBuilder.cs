using System;
using System.Drawing;
using QuickFox.Components;
using QuickFox.Managers;
using QuickFox.Rendering;

namespace QuickFox.UI
{
    public class UIElementBuilder<T> where T : UIEntity
    {
        private readonly IUIManager _uiManager;
        private readonly IComponentManager _componentManager;
        private readonly T _uiElement;

        internal UIElementBuilder(IUIManager uiManager, IComponentManager componentManager, string layerId, string sceneId, int order)
        {
            _uiManager = uiManager;
            _uiElement = _uiManager.CreateUIElement<T>();
            _uiElement.LayerId = layerId;
            _uiElement.SceneId = sceneId;
            _uiElement.OrderInParent = order;
            _componentManager = componentManager;
        }

        public UIElementBuilder<T> WithSprite(string resourceId)
        {
            var spriteComponent = _componentManager.CreateComponent<SpriteComponent>(_uiElement.Id);
            spriteComponent.ResourceId = resourceId;
            return this;
        }

        public UIElementBuilder<T> WithBackground(string resourceId)
        {
            var spriteComponent = _componentManager.CreateComponent<BackgroundComponent>(_uiElement.Id);
            spriteComponent.BackgroundResourceId = resourceId;
            return this;
        }

        public UIElementBuilder<T> WithSize(int width, int height)
        {
            _uiElement.WidthRequest = width;
            _uiElement.HeightRequest = height;
            return this;
        }

        public UIElementBuilder<T> WithMargins(int left, int top, int right, int bottom)
        {
            _uiElement.MarginLeft = left;
            _uiElement.MarginTop = top;
            _uiElement.MarginRight = right;
            _uiElement.MarginBottom = bottom;
            return this;
        }

        public UIElementBuilder<T> WithText(string text, Action<TextComponent> postCreate = null)
        {
            var textComponent = _componentManager.CreateComponent<TextComponent>(_uiElement.Id);
            textComponent.Text = text;
            postCreate?.Invoke(textComponent);
            return this;
        }

        public UIElementBuilder<T> WithClick<K>() where K : ClickBehaviorComponent
        {
            _componentManager.CreateComponent<K>(_uiElement.Id);
            _componentManager.CreateComponent<UserClickableComponent>(_uiElement.Id);
            return this;
        }

        public UIElementBuilder<T> AlignHorizontally(HorizontalAlignment alignment)
        {
            _uiElement.HorizontalAlignment = alignment;
            return this;
        }

        public UIElementBuilder<T> AlignVertically(VerticalAlignment alignment)
        {
            _uiElement.VerticalAlignment = alignment;
            return this;
        }

        public UIElementBuilder<T> WithClick(Action onClick)
        {
            var click = _componentManager.CreateComponent<ActionClickComponent>(_uiElement.Id);
            _componentManager.CreateComponent<UserClickableComponent>(_uiElement.Id);
            click.ClickAction = (c) => onClick.Invoke();
            return this;
        }

        public UIElementBuilder<T> WithClick(Action<ClickContext> onClick)
        {
            var click = _componentManager.CreateComponent<ActionClickComponent>(_uiElement.Id);
            _componentManager.CreateComponent<UserClickableComponent>(_uiElement.Id);
            click.ClickAction = onClick;
            return this;
        }

        public T Build()
        {
            return _uiElement;
        }
    }
}
