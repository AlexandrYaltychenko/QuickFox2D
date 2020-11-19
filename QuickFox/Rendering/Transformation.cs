using System;
namespace QuickFox.Rendering
{
    public struct Transformation
    {
        public float TranslationX { get; set; }

        public float TranslationY { get; set; }

        public float ScaleX { get; set; }

        public float ScaleY { get; set; }

        public float Rotation { get; set; }

        public static Transformation Identity()
        {
            return new Transformation
            {
                TranslationX = 0,
                TranslationY = 0,
                ScaleX = 1,
                ScaleY = 1,
                Rotation = 0
            };
        }

        public Transformation TranslateTo(Position position)
        {
            TranslationX = position.X;
            TranslationY = position.Y;
            return this;
        }

        public Transformation TranslateTo(float x, float y)
        {
            TranslationX = x;
            TranslationY = y;
            return this;
        }

        public Transformation Rotate(float angle)
        {
            Rotation = angle;
            return this;
        }

        public Transformation Scale(float scaleX, float scaleY)
        {
            ScaleX = scaleX;
            ScaleY = ScaleY;
            return this;
        }

    }
}
