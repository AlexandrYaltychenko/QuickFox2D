using System;
using QuickFox.Components;
using QuickFox.Managers;
using QuickFox.Rendering;
using QuickFox.UI;
using SkiaSharp;

namespace QuickFox.Skia
{
    public class SkiaMeasurementProvider : IMeasurementProvider
    {
        private readonly IEntityManager _entityManager;
        private readonly IComponentManager _componentManager;
        private readonly SKPaint _paint;

        public SkiaMeasurementProvider(IEntityManager entityManager, IComponentManager componentManager)
        {
            _entityManager = entityManager;
            _componentManager = componentManager;
            _paint = new SKPaint();
        }

        public Size Measure(IRenderableComponent component)
        {
            if (component is TextComponent textComponent)
            {
                _paint.TextSize = textComponent.TextSize;
                _paint.StrokeWidth = textComponent.StrokeWidth;
                _paint.Style = SKPaintStyle.Fill;

                var width = _paint.MeasureText(textComponent.Text);
                return new Size
                {
                    Width = width,
                    Height = textComponent.TextSize
                };
            }

            return new Size
            {
                Width = 0,
                Height = 0
            };
        }
    }
}
