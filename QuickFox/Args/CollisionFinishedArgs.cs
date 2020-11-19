using System;
using QuickFox.Components;

namespace QuickFox.Args
{
    public class CollisionFinishedArgs<T, S> : EventArgs where T : IEntity where S : IEntity
    {
        public T First { get; }
        public S Second { get; }

        public CollisionFinishedArgs(T first, S second)
        {
            this.First = first;
            this.Second = second;
        }
    }
}
