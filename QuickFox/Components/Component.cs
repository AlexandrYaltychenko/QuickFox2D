using System;
namespace QuickFox.Components
{
    public class Component : IComponent
    {
        public string Id { get; }

        public string EntityId { get; set; }

        public string Tag { get; set; }

        public Component()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
