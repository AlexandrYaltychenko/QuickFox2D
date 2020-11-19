using System;
using QuickFox.Components;
using QuickFox.Managers;
using QuickFox.Skia.Resources;
using QuickFox.States;
using SkiaSharp;

namespace QuickFox.Skia.Rendering
{
    public class TiledSpriteRenderer : BaseSkiaRenderer<TiledSpriteComponent>
    {
        private readonly IResourceManager _memoryManager;
        private readonly SKPaint _defaultPaint;

        public TiledSpriteRenderer(IResourceManager memoryManager)
        {
            _memoryManager = memoryManager;
            _defaultPaint = new SKPaint
            {
                FilterQuality = SKFilterQuality.High
            };
        }

        public override void Render(SKCanvas target, TiledSpriteComponent component, SKRect location, float alpha)
        {
            var texture = _memoryManager.GetTypedResource<SkiaTiledTextureResource>(component.ResourceId);
            var bitmap = texture.TiledBitmap;

            if (bitmap == null)
                return;

            var currentRow = component.Index / texture.Cols;
            var currentCol = component.Index % texture.Cols;
            var tileWidth = texture.TiledBitmap.Width / texture.Cols;
            var tileHeight = texture.TiledBitmap.Height / texture.Rows;
            var left = currentCol * tileWidth;
            var top = currentRow * tileHeight;

            target.DrawBitmap(bitmap, new SKRect(left, top, left + tileWidth, top + tileHeight), location, _defaultPaint);
        }
    }
}
