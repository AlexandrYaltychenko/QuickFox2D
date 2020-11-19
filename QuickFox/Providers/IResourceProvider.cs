using System;
using QuickFox.Resources;

namespace QuickFox.Providers
{
    public interface IResourceProvider
    {
        IResource LoadTexture(string resourceId, string resourceUri);

        IResource LoadAnimation(string resourceId, string resourceUri, int rows, int cols, int count);

        IResource LoadTiledTexture(string resourceId, string resourceUri, int rows, int cols, int count);
    }
}
