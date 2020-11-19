using System;
namespace QuickFox.TimerTasks
{
    public interface ITimerTask
    {
        string Id { get; }

        string SceneId { get; }

        string RefId { get; set; }

        Action<ITimerTask> Action { get; }

        bool Repeating { get; set; }

        long Interval { get; }

        long Remaining { get; set; }
    }
}
