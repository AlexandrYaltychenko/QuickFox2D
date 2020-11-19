using System;
namespace QuickFox.Components
{
    public interface IAnimatedComponent : IComponent
    {
        int Current { get; set; }
        int FrameDelay { get; set; }
        int FrameCount { get; }
        bool Animated { get; set; }
        int RemainingDelay { get; set; }
        bool IsRepeating { get; set; }
    }
}
