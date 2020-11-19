using System;
using QuickFox.Managers;
using QuickFox.Providers;
using QuickFox.Resources;
using SkiaSharp;

namespace QuickFox.Skia.Resources
{
    public class SkiaResourceProvider : IResourceProvider
    {
        private IFilesystemProvider _filesystemProvider;

        public SkiaResourceProvider(IFilesystemProvider filesystemProvider)
        {
            _filesystemProvider = filesystemProvider;
        }


        public IResource LoadAnimation(string resourceId, string resourceUri, int rows, int cols, int count)
        {
            var resourceBytes = _filesystemProvider.ReadResource(resourceUri);
            var bitmap = SKBitmap.Decode(resourceBytes);
            return new SkiaSpriteAnimationResource(resourceId, bitmap, bitmap.ByteCount)
            {
                Rows = rows,
                Cols = cols,
                Count = count
            };
        }

        public IResource LoadTexture(string resourceId, string resourceUri)
        {
            var resourceBytes = _filesystemProvider.ReadResource(resourceUri);
            var bitmap = SKBitmap.Decode(resourceBytes);
            return new SkiaTextureResource(resourceId, bitmap, bitmap.ByteCount);
        }

        public IResource LoadTiledTexture(string resourceId, string resourceUri, int rows, int cols, int count)
        {
            var resourceBytes = _filesystemProvider.ReadResource(resourceUri);
            var bitmap = SKBitmap.Decode(resourceBytes);
            return new SkiaTiledTextureResource(resourceId, bitmap, bitmap.ByteCount)
            {
                Rows = rows,
                Cols = cols,
                Count = count
            };
        }
    }
}
