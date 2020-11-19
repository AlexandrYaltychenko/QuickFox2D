using System;
using QuickFox.Systems;

namespace QuickFox.Managers
{
    public interface ISystemManager
    {
        void Register<T>(T system) where T : ISystem;

        void Unregister<T>(T system) where T : ISystem;

        void InvokeAll();

        void Invoke<T>() where T : ISystem;
    }
}
