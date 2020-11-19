using System;
namespace QuickFox.TimerTasks
{
    public class EntityTimerTask : TimerTask
    {
        public string EntityId { get; set; }

        public override string RefId { get => EntityId; set { EntityId = value; } }
    }
}
