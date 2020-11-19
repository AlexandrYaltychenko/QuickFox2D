using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using QuickFox.Managers;
using QuickFox.Providers;
using QuickFox.Resources;

namespace QuickFox.Engine
{
    public class ResourceManager : IResourceManager
    {
        private readonly IResourceProvider _resourceProvider;
        private readonly IDictionary<string, IResource> _resources;

        public ResourceManager(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
            _resources = new ConcurrentDictionary<string, IResource>();
        }

        public void LoadResource(string resourceId, string resourceUri)
        {
            var resource = _resourceProvider.LoadTexture(resourceId, resourceUri);
            _resources.Add(resource.Id, resource);
        }

        public void LoadAnimation(string resourceId, string resourceUri, int rows, int cols, int count)
        {
            var resource = _resourceProvider.LoadAnimation(resourceId, resourceUri, rows, cols, count);
            _resources.Add(resource.Id, resource);
        }

        public void LoadTiledResource(string resourceId, string resourceUri, int rows, int cols, int count)
        {
            var resource = _resourceProvider.LoadTiledTexture(resourceId, resourceUri, rows, cols, count);
            _resources.Add(resource.Id, resource);
        }

        public IResource GetResource(string resourceId)
        {
            return _resources[resourceId];
        }

        public T GetTypedResource<T>(string resourceId) where T : class
        {
            if (_resources.TryGetValue(resourceId, out var resource))
            {
                return resource as T;
            }
            else
            {
                return null;
            }
        }

        public void ReleaseResource(string resourceId)
        {
            _resources[resourceId].Dispose();
            _resources.Remove(resourceId);
        }
    }
}
