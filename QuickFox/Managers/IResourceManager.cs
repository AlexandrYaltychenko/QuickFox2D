using System;
using QuickFox.Resources;

namespace QuickFox.Managers
{
    public interface IResourceManager
    {
        void LoadResource(string resourceId, string resourceUri);
        void LoadAnimation(string resourceId, string resourceUri, int rows, int cols, int count);
        void LoadTiledResource(string resourceId, string resourceUri, int rows, int cols, int count);
        IResource GetResource(string resourceId);
        T GetTypedResource<T>(string resourceId) where T : class;
        void ReleaseResource(string resourceId);
    }
}
