using System;
using System.Diagnostics;
using QuickFox.Resources;

namespace QuickFox.Components
{
    public class AnimatedSpriteComponent : Component, IRenderableComponent, IAnimatedComponent
    {
        public string ResourceId { get; private set; }
        public int Current { get; set; }
        public int FrameDelay { get; set; } = 250;
        public int FrameCount { get; set; }
        public int Order { get; set; } = 1;
        public bool Animated { get; set; } = true;
        public int RemainingDelay { get; set; } = 0;
        public bool IsRepeating { get; set; } = true;

        public void SetResource(IAnimationResource animation)
        {
            FrameCount = animation.FrameCount;
            ResourceId = animation.Id;
        }
    }
}
