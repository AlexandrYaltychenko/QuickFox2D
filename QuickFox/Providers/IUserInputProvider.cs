using System;
using QuickFox.Args;
using QuickFox.Systems;

namespace QuickFox.Providers
{
    public interface IUserInputProvider
    {
        void Enable();

        void Disable();

        void Clear();

        IUserInputArgs Peek();

        IUserInputArgs Pop();

    }
}
