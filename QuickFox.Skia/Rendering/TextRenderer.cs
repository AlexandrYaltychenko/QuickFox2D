using System;
using QuickFox.Components;
using QuickFox.States;
using SkiaSharp;

namespace QuickFox.Skia.Rendering
{
    public class TextRenderer : BaseSkiaRenderer<TextComponent>
    {
        private readonly SKPaint _paint;

        public TextRenderer()
        {
            _paint = new SKPaint();
        }

        public override void Render(SKCanvas target, TextComponent component, SKRect location, float alpha)
        {
            var text = component.Text;

            if (string.IsNullOrWhiteSpace(text))
                return;

            _paint.TextSize = component.TextSize;
            _paint.Style = SKPaintStyle.Fill;
            _paint.StrokeWidth = component.StrokeWidth;
            _paint.Color = new SKColor(component.TextColor.R, component.TextColor.G, component.TextColor.B, (byte)Math.Min(alpha * 255, component.TextColor.A));

            target.DrawText(text, location.Left, location.Top + location.Height, _paint);
        }
    }
}
