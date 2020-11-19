using System;
using System.Collections.Generic;
using QuickFox.TimerTasks;

namespace QuickFox.Managers
{
    public interface ITimeManager
    {
        TimerTaskBuilder GetBuilder(string sceneId);

        void Attach(ITimerTask timerTask);

        ITimerTask Get(string id);

        IList<ITimerTask> GetActiveTasks();

        IList<ITimerTask> GetTasksForComponent(string componentId);

        IList<ITimerTask> GetTasksForEntity(string entityId);

        IList<ITimerTask> GetTasksForScene(string sceneId);

        void RemoveById(string taskId);

        void RemoveByComponentId(string taskId);

        void RemoveByEntityId(string taskId);

        void RemoveBySceneId(string sceneId);
    }
}
