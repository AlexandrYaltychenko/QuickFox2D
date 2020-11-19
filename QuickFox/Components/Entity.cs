using System;
namespace QuickFox.Components
{
    public class Entity : IEntity
    {
        public string Id { get; }

        public string SceneId { get; set; }

        internal Entity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
