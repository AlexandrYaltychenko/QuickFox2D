using System;
using System.Drawing;

namespace QuickFox.Rendering
{
    public struct Rect
    {
        public float X1 { get; set; }

        public float Y1 { get; set; }

        public float X2 { get; set; }

        public float Y2 { get; set; }

        public float Left => X1 > X2 ? X2 : X1;

        public float Top => Y1 > Y2 ? Y2 : Y1;

        public float Right => X1 > X2 ? X1 : X2;

        public float Width => Math.Abs(X2 - X1);

        public float Height => Math.Abs(Y2 - Y1);

        public float MidX => X1 + Width / 2;

        public float MidY => Y1 + Height / 2;
    }

    public static class RectExtensions
    {
        public static Rect ToRect(this RectangleF rect)
        {
            var result = new Rect();
            result.X1 = rect.Left;
            result.X2 = rect.Right;
            result.Y1 = rect.Top;
            result.Y2 = rect.Bottom;

            return result;
        }
    }
}
