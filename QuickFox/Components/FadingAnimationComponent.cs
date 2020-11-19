using System;
namespace QuickFox.Components
{
    public class FadingAnimationComponent : AlphaComponent, IAnimatedComponent
    {
        private bool _fading;

        public int Current { get; set; }

        public int FrameDelay { get; set; } = 25;

        public int FrameCount { get; set; }

        public bool Animated { get; set; } = true;

        public int RemainingDelay { get; set; }

        public override float Alpha { get => GetCurrentAlpha(); set { } }

        public float ToAlpha { get; set; }

        public float FromAlpha { get; set; }

        public bool IsRepeating { get; set; }

        public void Setup(float fromAlpha, float toAlpha, float time)
        {
            FrameCount = (int)(time / FrameDelay);
            RemainingDelay = FrameDelay;
            FromAlpha = fromAlpha;
            ToAlpha = toAlpha;
            _fading = FromAlpha > ToAlpha;
        }

        private float GetCurrentAlpha()
        {
            if (!_fading)
            {
                return FromAlpha + (ToAlpha - FromAlpha) / FrameCount * Current;
            }
            else
            {
                return ToAlpha - (FromAlpha - ToAlpha) / FrameCount * Current;
            }
        }

    }
}
