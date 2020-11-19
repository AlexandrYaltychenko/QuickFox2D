using System;
using QuickFox.Args;

namespace QuickFox.Managers
{
    public interface IEventManager
    {
        public void ReleaseSubscriptions<T>() where T : EventArgs;

        public IEventSubscription Subscribe<T>(Action<T> handler) where T : EventArgs;

        public void Post<T>(T eventArgs) where T : EventArgs;
    }
}
