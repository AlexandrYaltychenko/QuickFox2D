using System;
using System.Drawing;

namespace QuickFox.Components
{
    public class BackgroundComponent : Component, IRenderableComponent
    {
        public string BackgroundResourceId { get; set; }

        public Color BackgroundColor { get; set; }

        public int Order { get; set; } = 0;
    }
}
