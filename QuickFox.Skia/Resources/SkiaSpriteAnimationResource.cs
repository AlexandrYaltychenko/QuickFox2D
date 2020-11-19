using QuickFox.Resources;
using SkiaSharp;

namespace QuickFox.Skia.Resources
{
    public class SkiaSpriteAnimationResource : Resource, IAnimationResource
    {
        private SKBitmap _animation;

        public int Count { get; set; }

        public int Rows { get; set; }

        public int Cols { get; set; }

        public int FrameCount => Count;

        public SkiaSpriteAnimationResource(string resourceId, SKBitmap bitmap, long byteCount) : base(resourceId)
        {
            _animation = bitmap;
            SizeInBytes = byteCount;
        }

        public SKBitmap TiledBitmap => _animation;

        public override void Dispose()
        {
            _animation?.Dispose();
        }
    }
}
