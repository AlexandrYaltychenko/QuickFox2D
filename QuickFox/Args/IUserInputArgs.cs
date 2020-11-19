using System;
namespace QuickFox.Args
{
    public interface IUserInputArgs
    {
        InputType InputType { get; }
    }

    public enum InputType
    {
        Keyboard, Mouse, Touch
    }
}
