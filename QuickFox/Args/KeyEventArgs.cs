using System;
namespace QuickFox.Args
{
    public class KeyEventArgs : EventArgs, IUserInputArgs
    {
        public KeyState State { get; }

        public KeyAction Action { get; }

        public int Code { get; }

        public InputType InputType => InputType.Keyboard;

        public KeyEventArgs(KeyState state, KeyAction action, int code)
        {
            State = state;
            Action = action;
            Code = code;
        }
    }

    public enum KeyState
    {
        Up, Down
    }

    public enum KeyAction
    {
        Left, Right, Up, Down, Plus, Minus, Other
    }
}
