using System;
using System.Collections.Generic;
using System.Linq;
using QuickFox.TimerTasks;

namespace QuickFox.Managers
{
    public class TimeManager : ITimeManager
    {
        private readonly ISet<ITimerTask> _tasks;

        public TimeManager()
        {
            _tasks = new HashSet<ITimerTask>();
        }

        public void Attach(ITimerTask timerTask)
        {
            _tasks.Add(timerTask);
        }

        public ITimerTask Get(string id)
        {
            return _tasks.Where(t => t.Id == id).FirstOrDefault();
        }

        public IList<ITimerTask> GetActiveTasks()
        {
            return _tasks.Where(t => t.Repeating || t.Remaining > 0).ToList();
        }

        public TimerTaskBuilder GetBuilder(string sceneId)
        {
            return new TimerTaskBuilder(this, sceneId);
        }

        public IList<ITimerTask> GetTasksForComponent(string componentId)
        {
            return _tasks.OfType<ComponentTimerTask>().Where(c => c.RefId == componentId).OfType<ITimerTask>().ToList();
        }

        public IList<ITimerTask> GetTasksForEntity(string entityId)
        {
            return _tasks.OfType<EntityTimerTask>().Where(e => e.RefId == entityId).OfType<ITimerTask>().ToList();
        }

        public IList<ITimerTask> GetTasksForScene(string sceneId)
        {
            return _tasks.Where(t => t.SceneId == sceneId).ToList();
        }

        public void RemoveByComponentId(string componentId)
        {
            var tasks = GetTasksForComponent(componentId);
            if (tasks.Any())
            {
                foreach (var task in tasks)
                {
                    _tasks.Remove(task);
                }
            }
        }

        public void RemoveByEntityId(string entityId)
        {
            var tasks = GetTasksForEntity(entityId);
            if (tasks.Any())
            {
                foreach (var task in tasks)
                {
                    _tasks.Remove(task);
                }
            }
        }

        public void RemoveById(string id)
        {
            var task = Get(id);
            if (task != null)
                _tasks.Remove(task);
        }

        public void RemoveBySceneId(string sceneId)
        {
            var tasks = GetTasksForScene(sceneId);

            if (tasks.Any())
            {
                foreach (var task in tasks)
                {
                    _tasks.Remove(task);
                }
            }
        }
    }
}
