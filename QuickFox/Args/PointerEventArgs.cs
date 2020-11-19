using System;
namespace QuickFox.Args
{
    public class PointerEventArgs : EventArgs
    {
        public float X { get; set; }
        public float Y { get; set; }
        public PointerActionType ActionType { get; set; }
    }

    public enum PointerActionType
    {
        Up, Down, Move
    }
}
