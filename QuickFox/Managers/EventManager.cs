using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using QuickFox.Args;
using QuickFox.Extensions;

namespace QuickFox.Managers
{
    public class EventManager : IEventManager
    {
        private readonly IDictionary<Type, ISet<IManagedEventSubscription>> _subscriptions;

        public EventManager()
        {
            _subscriptions = new Dictionary<Type, ISet<IManagedEventSubscription>>();
        }

        public void Post<T>(T eventArgs) where T : EventArgs
        {
            if (_subscriptions.TryGetValue(typeof(T), out var topicSubscriptions))
            {
                topicSubscriptions.ForEach(s => s.Deliver(eventArgs));
            }
        }

        public void ReleaseSubscriptions<T>() where T : EventArgs
        {
            if (_subscriptions.TryGetValue(typeof(T), out var topicSubscriptions))
            {
                topicSubscriptions.ForEach(s => s.Dispose());
                topicSubscriptions.Clear();
            }
        }

        public IEventSubscription Subscribe<T>(Action<T> handler) where T : EventArgs
        {
            var topicType = typeof(T);
            ISet<IManagedEventSubscription> subscriptionsForTopic;
            if (!_subscriptions.TryGetValue(topicType, out subscriptionsForTopic))
            {
                subscriptionsForTopic = new HashSet<IManagedEventSubscription>();
                _subscriptions.TryAdd(topicType, subscriptionsForTopic);
            }

            var newSubscription = new EventSubscription<T>(handler);
            subscriptionsForTopic.Add(newSubscription);

            return newSubscription;
        }
    }
}
