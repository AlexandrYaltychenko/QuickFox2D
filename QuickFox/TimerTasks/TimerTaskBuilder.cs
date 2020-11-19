using System;
using QuickFox.Components;
using QuickFox.Managers;

namespace QuickFox.TimerTasks
{
    public class TimerTaskBuilder
    {
        private readonly ITimeManager _timeManager;
        private long _interval;
        private long _firstDelay;
        private string _refId;
        private string _sceneId;
        private Action<ITimerTask> _action;
        private TimerTaskType _taskType;
        private bool _repeating;
        
        public TimerTaskBuilder(ITimeManager timeManager, string sceneId)
        {
            _timeManager = timeManager;
            _sceneId = sceneId;
        }

        public TimerTaskBuilder WithInterval(long interval)
        {
            _interval = interval;
            return this;
        }

        public TimerTaskBuilder WithDelay(long delay)
        {
            _firstDelay = delay;
            return this;
        }

        public TimerTaskBuilder WithAction(Action<ITimerTask> action)
        {
            _action = action;
            return this;
        }

        public TimerTaskBuilder ForComponent(IComponent component)
        {
            _refId = component.Id;
            _taskType = TimerTaskType.Component;
            return this;
        }

        public TimerTaskBuilder ForEntity(IEntity entity)
        {
            _refId = entity.Id;
            _taskType = TimerTaskType.Entity;
            return this;
        }

        public TimerTaskBuilder Repeat()
        {
            _repeating = true;
            return this;
        }

        public ITimerTask Build()
        {
            TimerTask timeTask =  _taskType == TimerTaskType.Component ? (TimerTask) new ComponentTimerTask() : new EntityTimerTask();
            timeTask.RefId = _refId;
            timeTask.Action = _action;
            timeTask.Interval = _interval;
            timeTask.Remaining = _firstDelay;
            timeTask.SceneId = _sceneId;
            timeTask.Repeating = _repeating;
            _timeManager.Attach(timeTask);

            return timeTask;
        }

        private enum TimerTaskType
        {
            Entity, Component
        }
    }
}
