using System;
namespace QuickFox.TimerTasks
{
    public abstract class TimerTask : ITimerTask
    {
        public string Id { get; }
        public string SceneId { get; set; }
        public Action<ITimerTask> Action { get; set; }
        public bool Repeating { get; set; }
        public long Interval { get; set; }
        public long Remaining { get; set; }
        public abstract string RefId { get; set; }

        public TimerTask()
        {
            Id = Guid.NewGuid().ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is TimerTask task && task.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
