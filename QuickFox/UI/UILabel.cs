using System;
using QuickFox.Components;
using QuickFox.Managers;

namespace QuickFox.UI
{
    public class UILabel : UIEntity
    {
        private readonly IComponentManager _componentManager;
        private readonly IMeasurementProvider _measurementProvider;
        private readonly IEntityManager _entityManager;

        public UILabel(IEntityManager entityManager, IComponentManager componentManager, IMeasurementProvider measurementProvider) : base(entityManager)
        {
            _componentManager = componentManager;
            _measurementProvider = measurementProvider;
        }

        public string Text { get => GetText(); set { SetText(value); } }

        internal override void Measure(int availableWidth, int availableHeight)
        {
            base.Measure(availableWidth, availableHeight);

            if (WidthRequest == LayoutConstants.WrapContent)
            {
                var textComponent = _componentManager.GetComponentForEntity<TextComponent>(Id);
                if (textComponent != null)
                {
                    var size = _measurementProvider.Measure(textComponent);
                    Bounds.Width = (int)Math.Min(availableWidth, size.Width);
                    if (HeightRequest == LayoutConstants.WrapContent)
                    {
                        Bounds.Height = (int)Math.Min(availableHeight, size.Height);
                    }
                }
            }
        }

        private string GetText()
        {
            var textComponent = _componentManager.GetComponentForEntity<TextComponent>(Id);
            return textComponent?.Text ?? null;
        }

        private void SetText(string value)
        {
            var textComponent = _componentManager.GetComponentForEntity<TextComponent>(Id);
            if (textComponent != null && textComponent.Text != value)
            {
                textComponent.Text = value;
                Invalidate();
            }
        }

    }
}
