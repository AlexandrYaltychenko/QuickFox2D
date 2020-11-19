using System;
using System.Diagnostics;
using QuickFox.Args;
using QuickFox.Managers;

namespace QuickFox.Systems
{
    public class StatsSystem : IStatsSystem
    {
        private const int FpsMaxCount = 30;
        private readonly IEventManager _eventManager;
        private readonly Stopwatch _stopWatch;
        private int _fpsCounter;
        private long _timeCounter;
        public float CurrentFPS { get; private set; }

        public StatsSystem(IEventManager eventManager)
        {
            _stopWatch = new Stopwatch();
            _eventManager = eventManager;
            _eventManager.Subscribe<SceneUpdatedArgs>(OnSceneUpdated);
        }

        private void OnSceneUpdated(SceneUpdatedArgs args)
        {
            _timeCounter += _stopWatch.ElapsedMilliseconds;
            _fpsCounter++;

            if (_fpsCounter >= FpsMaxCount)
            {
                var avgFpsTime = _timeCounter / _fpsCounter;
                if (avgFpsTime > 0)
                {
                    CurrentFPS = 1000f / avgFpsTime;
                }
                else
                {
                    CurrentFPS = 0;
                }

                _fpsCounter = 0;
                _timeCounter = 0;
            }

            _stopWatch.Restart();
        }
    }
}
