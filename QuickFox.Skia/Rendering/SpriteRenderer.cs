using System;
using QuickFox.Components;
using QuickFox.Managers;
using QuickFox.Rendering;
using QuickFox.Skia.Resources;
using QuickFox.States;
using SkiaSharp;

namespace QuickFox.Skia.Rendering
{
    public class SpriteRenderer : BaseSkiaRenderer<SpriteComponent>
    {
        private readonly IResourceManager _memoryManager;
        private readonly SKPaint _defaultPaint;

        public SpriteRenderer(IResourceManager memoryManager)
        {
            _memoryManager = memoryManager;
            _defaultPaint = new SKPaint
            {
                FilterQuality = SKFilterQuality.High
            };
        }

        public override void Render(SKCanvas target, SpriteComponent component, SKRect location, float alpha)
        {
            var texture = _memoryManager.GetTypedResource<SkiaTextureResource>(component.ResourceId);
            var bitmap = texture.TextureBitmap;

            if (bitmap == null)
                return;


            if (Math.Abs(alpha - 1f) < 0.99f)
            {
                target.DrawBitmap(bitmap, location, _defaultPaint);
            }
            else
            {
                using (var colorFilter = SKColorFilter.CreateBlendMode(SKColors.White.WithAlpha((byte)(alpha * byte.MaxValue)), SKBlendMode.DstIn))
                {
                    _defaultPaint.ColorFilter = colorFilter;
                    target.DrawBitmap(bitmap, location, _defaultPaint);
                    _defaultPaint.ColorFilter = null;
                }
            }
        }
    }
}
