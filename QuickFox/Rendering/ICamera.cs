using System;
namespace QuickFox.Rendering
{
    public interface ICamera
    {
        float X { get; }

        float Y { get; }

        float Z { get; }

        void Update();

        void ZoomTo(float coef);

        void ZoomBy(float coef);
    }
}
