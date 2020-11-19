using System;
namespace QuickFox.Components
{
    public class ScheduledDestroyComponent : Component
    {
        public long RemainingTime { get; set; }

        public bool ShouldBeDestroyed => RemainingTime <= 0;
    }
}
