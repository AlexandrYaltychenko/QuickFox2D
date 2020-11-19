using System;
namespace QuickFox.Resources
{
    public interface IResource : IDisposable
    {
        string Id { get; }

        long SizeInBytes { get; }

        bool IsDisposed { get; }
    }
}
