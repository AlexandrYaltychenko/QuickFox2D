using System;
using System.Collections.Generic;
using System.Linq;
using QuickFox.Extensions;
using QuickFox.Systems;

namespace QuickFox.Managers
{
    public class SystemManager : ISystemManager
    {
        private ISet<ISystem> _systems = new HashSet<ISystem>();

        public void Invoke<T>() where T : ISystem
        {
            //_systems.OfType<T>().ForEach(system => system.Invoke());
        }

        public void InvokeAll()
        {
            //_systems.ForEach(system => system.Invoke());
        }

        public void Register<T>(T system) where T : ISystem
        {
            _systems.Add(system);
        }

        public void Unregister<T>(T system) where T : ISystem
        {
            _systems.Remove(system);
        }
    }
}
