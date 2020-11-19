using System;
using QuickFox.Components;
using QuickFox.Managers;
using QuickFox.Skia.Resources;
using QuickFox.States;
using SkiaSharp;

namespace QuickFox.Skia.Rendering
{
    public class BackgroundRenderer : BaseSkiaRenderer<BackgroundComponent>
    {
        private readonly IResourceManager _memoryManager;
        private readonly SKPaint _defaultPaint;

        public BackgroundRenderer(IResourceManager memoryManager)
        {
            _memoryManager = memoryManager;
            _defaultPaint = new SKPaint
            {
                FilterQuality = SKFilterQuality.High
            };
        }

        public override void Render(SKCanvas target, BackgroundComponent component, SKRect location, float alpha)
        {
            var texture = _memoryManager.GetTypedResource<SkiaTextureResource>(component.BackgroundResourceId);
            var bitmap = texture.TextureBitmap;

            if (bitmap == null)
                return;

            target.DrawBitmap(bitmap, location, _defaultPaint);
        }
    }
}