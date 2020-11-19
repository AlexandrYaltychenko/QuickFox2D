using System;
using System.Collections.Generic;
using QuickFox.Components;

namespace QuickFox.UI
{
    public struct UIRenderingTarget
    {
        public UIBounds Bounds { get; set; }

        public IList<IRenderableComponent> RenderableComponents { get; set; }
    }
}
