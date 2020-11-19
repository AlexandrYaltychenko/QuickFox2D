using System;
namespace QuickFox.Args
{
    public class EventSubscription<T> : IManagedEventSubscription
    {
        public Action<T> Action { get; private set; }

        public EventSubscription(Action<T> handlerAction)
        {
            Action = handlerAction;
        }

        public void Dispose()
        {
            Action = null;
        }

        public void Deliver(EventArgs eventArgs)
        {
            if (eventArgs is T typedArgs)
            {
                Action?.Invoke(typedArgs);
            }
        }
    }
}
