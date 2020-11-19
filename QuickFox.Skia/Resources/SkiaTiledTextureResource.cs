using System;
using QuickFox.Resources;
using SkiaSharp;

namespace QuickFox.Skia.Resources
{
    public class SkiaTiledTextureResource : Resource
    {
        private SKBitmap _bitmap;

        public int Count { get; set; }

        public int Rows { get; set; }

        public int Cols { get; set; }

        public SkiaTiledTextureResource(string resourceId, SKBitmap bitmap, long byteCount) : base(resourceId)
        {
            _bitmap = bitmap;
            SizeInBytes = byteCount;
        }

        public SKBitmap TiledBitmap => _bitmap;

        public override void Dispose()
        {
            _bitmap?.Dispose();
        }
    }
}
