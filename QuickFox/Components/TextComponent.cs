using System;
using System.Drawing;

namespace QuickFox.Components
{
    public class TextComponent : Component, IRenderableComponent
    {
        public string Text { get; set; }

        public int TextSize { get; set; } = 16;

        public int StrokeWidth { get; set; } = 2;

        public Color TextColor { get; set; } = Color.White;

        public int Order { get; set; } = 10;
    }
}
