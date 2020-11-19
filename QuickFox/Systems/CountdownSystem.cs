using System;
using System.Diagnostics;
using QuickFox.Args;
using QuickFox.Managers;

namespace QuickFox.Systems
{
    public class CountdownSystem : ICountdownSystem
    {
        protected readonly IEventManager EventManager;
        protected readonly ITimeManager TimeManager;
        protected Stopwatch _sceneStopwatch;

        public CountdownSystem(ITimeManager timeManager, IEventManager eventManager)
        {
            TimeManager = timeManager;
            EventManager = eventManager;
            _sceneStopwatch = new Stopwatch();
            EventManager.Subscribe<SceneUpdatedArgs>(OnSceneUpdated);
            EventManager.Subscribe<SceneChangedArgs>(OnSceneChanged);
        }

        private void OnSceneUpdated(SceneUpdatedArgs args)
        {
            _sceneStopwatch.Stop();

            if (args.Scene.State != States.SceneState.Playing)
            {
                return;
            }

            var tasks = TimeManager.GetTasksForScene(args.Scene.Id);

            foreach (var task in tasks)
            {
                if (task.Remaining >= 0)
                {
                    task.Remaining -= _sceneStopwatch.ElapsedMilliseconds;
                    if (task.Remaining < 0)
                    {
                        task.Action?.Invoke(task);
                        if (task.Repeating)
                        {
                            task.Remaining = task.Interval;
                        }
                        else
                        {
                            TimeManager.RemoveById(task.Id);
                        }
                    }
                }
            }

            _sceneStopwatch.Restart();
        }

        private void OnSceneChanged(SceneChangedArgs args)
        {
            _sceneStopwatch.Reset();
        }

    }
}
