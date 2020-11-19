using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using QuickFox.Args;

namespace QuickFox.Providers
{
    public abstract class BaseUserInputProvider : IUserInputProvider
    {
        protected Queue<IUserInputArgs> _queue;

        public BaseUserInputProvider()
        {
            _queue = new Queue<IUserInputArgs>();
        }

        public void Clear()
        {
            _queue.Clear();
        }

        public abstract void Disable();
        public abstract void Enable();

        public IUserInputArgs Peek()
        {
            if (_queue.TryPeek(out var result))
            {
                return result;
            }
            else return null;
        }

        public IUserInputArgs Pop()
        {
            if (_queue.TryDequeue(out var result))
            {
                return result;
            }
            else return null;
        }
    }
}
