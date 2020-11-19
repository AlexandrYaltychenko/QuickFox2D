using System;
using QuickFox.Components;
using QuickFox.Rendering;

namespace QuickFox.Collisions
{
    public class Collision
    {
        public IEntity Current { get; }
        public IEntity Target { get; }
        public ColliderBoxComponent Collider { get; }
        public ColliderBoxComponent TargetCollider { get; }
        public Rect Area { get; }

        public Collision(IEntity current, IEntity target, ColliderBoxComponent collider, ColliderBoxComponent targetCollider, Rect area)
        {
            Current = current;
            Target = target;
            Collider = collider;
            TargetCollider = targetCollider;
            Area = area;
        }
    }
}
