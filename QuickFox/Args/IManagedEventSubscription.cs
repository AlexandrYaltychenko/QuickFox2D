using System;
namespace QuickFox.Args
{
    public interface IManagedEventSubscription : IEventSubscription
    {
        void Deliver(EventArgs eventArgs);
    }
}
