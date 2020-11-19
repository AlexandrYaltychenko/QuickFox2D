using System;
namespace QuickFox.TimerTasks
{
    public class ComponentTimerTask : TimerTask
    {
        public string ComponentId { get; set; }

        public override string RefId { get => ComponentId; set { ComponentId = value; } }
    }
}
