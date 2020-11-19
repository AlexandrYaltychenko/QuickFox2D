using System;
using QuickFox.Components;
using QuickFox.Managers;
using QuickFox.Skia.Resources;
using QuickFox.States;
using SkiaSharp;

namespace QuickFox.Skia.Rendering
{
    public class AnimatedSpriteRenderer : BaseSkiaRenderer<AnimatedSpriteComponent>
    {
        private readonly IResourceManager _memoryManager;
        private readonly SKPaint _defaultPaint;

        public AnimatedSpriteRenderer(IResourceManager memoryManager)
        {
            _memoryManager = memoryManager;
            _defaultPaint = new SKPaint
            {
                FilterQuality = SKFilterQuality.High
            };
        }

        public override void Render(SKCanvas target, AnimatedSpriteComponent component, SKRect location, float alpha)
        {
            var texture = _memoryManager.GetTypedResource<SkiaSpriteAnimationResource>(component.ResourceId);
            var bitmap = texture.TiledBitmap;

            if (bitmap == null)
                return;

            var currentRow = component.Current / texture.Cols;
            var currentCol = component.Current % texture.Cols;
            var tileWidth = texture.TiledBitmap.Width / texture.Cols;
            var tileHeight = texture.TiledBitmap.Height / texture.Rows;
            var left = currentCol * tileWidth;
            var top = currentRow * tileHeight;

            if (Math.Abs(alpha - 1f) < 0.999f)
            {
                target.DrawBitmap(bitmap, new SKRect(left, top, left + tileWidth, top + tileHeight), location, _defaultPaint);
            }
            else
            {
                using (var colorFilter = SKColorFilter.CreateBlendMode(SKColors.White.WithAlpha((byte)(alpha * byte.MaxValue)), SKBlendMode.DstIn))
                {
                    _defaultPaint.ColorFilter = colorFilter;
                    target.DrawBitmap(bitmap, new SKRect(left, top, left + tileWidth, top + tileHeight), location, _defaultPaint);
                    _defaultPaint.ColorFilter = null;
                }
            }
        }
    }
}
