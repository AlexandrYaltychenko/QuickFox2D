using System;
using QuickFox.Resources;
using SkiaSharp;

namespace QuickFox.Skia.Resources
{
    public class SkiaGraphicsResource : Resource
    {
        private readonly SKPath _path;

        public SkiaGraphicsResource(string id, SKPath path) : base(id)
        {
            _path = path;
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                _path.Dispose();
                IsDisposed = true;
            }
        }
    }
}
