using System;
using QuickFox.Managers;

namespace QuickFox.Rendering
{
    public interface ISurface
    {
        float Width { get; }

        float Height { get; }

        void Attach(IEventManager eventManager);
    }
}
