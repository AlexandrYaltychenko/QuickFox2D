using System;
using QuickFox.Components;

namespace QuickFox.Args
{
    public class CollisionContinuesArgs<T, S> : EventArgs where T : IEntity where S : IEntity
    {
        public T First { get; }
        public S Second { get; }

        public CollisionContinuesArgs(T first, S second)
        {
            this.First = first;
            this.Second = second;
        }
    }
}
