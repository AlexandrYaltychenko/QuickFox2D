using System;
using System.Collections.Generic;

namespace QuickFox.Components
{
    public interface IEntity
    {
        string Id { get; }

        string SceneId { get; set; }
    }
}
