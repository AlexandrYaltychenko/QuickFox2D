using System;
using QuickFox.Components;
using QuickFox.States;
using SkiaSharp;

namespace QuickFox.Skia.Rendering
{
    public class HealthRenderer : BaseSkiaRenderer<HealthComponent>
    {
        private const int Height = 8;
        private const int MarginTop = 4;
        private const int StrokeWidth = 2;
        private readonly SKPaint _borderPaint;
        private readonly SKPaint _healthPaint;

        public HealthRenderer()
        {
            _borderPaint = new SKPaint
            {
                Color = SKColors.Black,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = StrokeWidth,
                IsAntialias = true
            };

            _healthPaint = new SKPaint
            {
                Color = SKColors.Green,
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };
        }

        public override void Render(SKCanvas target, HealthComponent component, SKRect location, float alpha)
        {
            var barExternalLocation = new SKRect(location.Left, location.Top - Height - MarginTop, location.Right, location.Top - MarginTop);

            var relativeHealth = (float)component.CurrentHealth / component.MaxHealth;

            target.DrawRect(barExternalLocation, _borderPaint);
            target.DrawRect(new SKRect(barExternalLocation.Left, barExternalLocation.Top, barExternalLocation.Left + barExternalLocation.Width * relativeHealth, barExternalLocation.Bottom), _healthPaint);
        }
    }
}
