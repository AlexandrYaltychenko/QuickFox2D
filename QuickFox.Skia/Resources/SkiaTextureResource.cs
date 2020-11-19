using System;
using QuickFox.Resources;
using SkiaSharp;

namespace QuickFox.Skia.Resources
{
    public class SkiaTextureResource : TextureResource
    {
        public SKBitmap TextureBitmap => _textureObject as SKBitmap;

        public SkiaTextureResource(string id, IDisposable resourceObject, long byteSize) : base(id, resourceObject, byteSize)
        {
        }
    }
}
