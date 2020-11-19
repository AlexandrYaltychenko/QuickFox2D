using System;
using QuickFox.Rendering;

namespace QuickFox.Args
{
    public class SceneUpdatedArgs : EventArgs
    {
        public Scene Scene { get; }

        public Rect SceneViewport { get; }

        public Rect ScreenViewport { get; }

        public SceneUpdatedArgs(Scene scene, Rect viewport, Rect screenViewport)
        {
            Scene = scene;
            SceneViewport = viewport;
            ScreenViewport = screenViewport;
        }
    }
}
