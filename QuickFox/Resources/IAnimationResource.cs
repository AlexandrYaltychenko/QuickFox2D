using System;
namespace QuickFox.Resources
{
    public interface IAnimationResource : IResource
    {
        int FrameCount { get; }
    }
}
