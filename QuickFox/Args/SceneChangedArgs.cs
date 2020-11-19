using System;
using QuickFox.Rendering;

namespace QuickFox.Args
{
    public class SceneChangedArgs : EventArgs
    {
        public Scene Scene { get; }

        public SceneChangedArgs(Scene scene)
        {
            Scene = scene;
        }
    }
}
