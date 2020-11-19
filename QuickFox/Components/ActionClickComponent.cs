using System;
using QuickFox.Managers;
using QuickFox.Rendering;

namespace QuickFox.Components
{
    public class ActionClickComponent : ClickBehaviorComponent
    {
        public Action<ClickContext> ClickAction { get; set; }

        public ActionClickComponent(IEntityManager entityManager, IComponentManager componentManager) : base(entityManager, componentManager)
        {
        }

        public override void OnClick(Point position, Scene scene)
        {
            ClickAction?.Invoke(new ClickContext(position, EntityId, Id));
        }
    }


    public class ClickContext
    {
        public Point Position { get; }

        public string EntityId { get; }

        public string ComponentId { get; }

        public ClickContext(Point position, string entityId, string componentId)
        {
            Position = position;
            EntityId = entityId;
            ComponentId = componentId;
        }

    }

}
