using System;
namespace QuickFox.Components
{
    public interface IComponent
    {
        string Id { get; }

        string EntityId { get; set; }

        string Tag { get; set; }
    }
}
