using System;
namespace QuickFox.Resources
{
    public class TextureResource : Resource
    {
        protected readonly IDisposable _textureObject;

        public TextureResource(string id, IDisposable resourceObject, long byteSize) : base(id)
        {
            _textureObject = resourceObject;
            SizeInBytes = byteSize;
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                _textureObject.Dispose();
                IsDisposed = true;
            }
        }
    }
}
