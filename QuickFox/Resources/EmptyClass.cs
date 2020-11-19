using System;
namespace QuickFox.Resources
{
    public abstract class Resource : IResource
    {
        public string Id { get; }

        public long SizeInBytes { get; protected set; }

        public bool IsDisposed { get; protected set; }

        public abstract void Dispose();

        public Resource(string id)
        {
            Id = id;
        }
    }
}
