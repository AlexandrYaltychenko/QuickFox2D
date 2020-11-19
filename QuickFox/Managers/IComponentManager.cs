using System;
using System.Collections.Generic;
using QuickFox.Components;

namespace QuickFox.Managers
{
    public interface IComponentManager
    {
        bool AttachComponent<T>(T component) where T : class, IComponent;

        T CreateComponent<T>(string entityId) where T : class, IComponent;

        void DeleteComponentsOfEntity(string entityId);

        void Delete(string componentId);

        IList<IComponent> GetComponentsForEntity(string entityId);

        T GetComponentForEntity<T>(string entityId) where T : class, IComponent;

        IList<IComponent> GetAllComponents();

        T GetComponent<T>(string componentId) where T : class, IComponent;

        IList<T> GetComponents<T>();

        void DeleteComponents<T>(Func<T, bool> predicate) where T : class, IComponent;
    }
}
