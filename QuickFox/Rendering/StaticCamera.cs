using System;
namespace QuickFox.Rendering
{
    public class StaticCamera : ICamera
    {
        public float X { get; set; } = 0f;

        public float Y { get; set; } = 0f;

        public float Z { get; set; } = 1f;

        public StaticCamera()
        {
        }

        public override string ToString()
        {
            return $"{X}x{Y}:{Z}";
        }

        public void Update()
        {
        }

        public void ZoomTo(float coef)
        {
            Z = coef;
        }

        public void ZoomBy(float coef)
        {
            Z *= coef;
        }
    }
}
