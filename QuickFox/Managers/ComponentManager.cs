using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using QuickFox.Components;
using Unity;

namespace QuickFox.Managers
{
    public class ComponentManager : IComponentManager
    {
        private IDictionary<string, IList<IComponent>> _components;
        private IDictionary<string, IComponent> _flatComponentDict;

        public ComponentManager()
        {
            _components = new ConcurrentDictionary<string, IList<IComponent>>();
            _flatComponentDict = new ConcurrentDictionary<string, IComponent>();
        }

        public T CreateComponent<T>(string entityId) where T : class, IComponent
        {
            T component = Fox.Container.Resolve<T>();
            component.EntityId = entityId;

            AddComponentToEntityList(entityId, component);

            _flatComponentDict[component.Id] = component;

            return component;
        }

        public IList<IComponent> GetAllComponents()
        {
            return _flatComponentDict.Values.ToList();
        }

        public T GetComponent<T>(string componentId) where T : class, IComponent
        {
            if (_flatComponentDict.TryGetValue(componentId, out var component))
            {
                return component as T;
            }

            return null;
        }

        public IList<T> GetComponents<T>()
        {
            return _flatComponentDict.Values.OfType<T>().ToList();
        }

        public IList<IComponent> GetComponentsForEntity(string entityId)
        {
            if (_components.TryGetValue(entityId, out var components))
            {
                return components;
            }
            else return new List<IComponent>();
        }

        public T GetComponentForEntity<T>(string entityId) where T: class, IComponent
        {
            return GetComponentsForEntity(entityId).OfType<T>().FirstOrDefault();
        }

        public void DeleteComponentsOfEntity(string entityId)
        {
            if (_components.Remove(entityId, out var components))
            {
                foreach (var component in components)
                {
                    _flatComponentDict.Remove(component.Id);
                }

                components.Clear();
            }
        }

        public void Delete(string componentId)
        {
            if (_flatComponentDict.Remove(componentId, out var component))
            {
                if (_components.TryGetValue(component.EntityId, out var components))
                {
                    components.Remove(component);
                }
            }
        }

        public bool AttachComponent<T>(T component) where T : class, IComponent
        {
            if (string.IsNullOrEmpty(component.EntityId))
            {
                throw new ArgumentException("Can't attach a component without entity!");
            }

            if (_flatComponentDict.ContainsKey(component.Id))
                return false;

            _flatComponentDict.Add(component.Id, component);
            AddComponentToEntityList(component.EntityId, component);
            return true;
        }

        private void AddComponentToEntityList(string entityId, IComponent component)
        {
            IList<IComponent> componentsOfEntity;

            if (!_components.TryGetValue(entityId, out componentsOfEntity))
            {
                componentsOfEntity = new List<IComponent>();
                _components[entityId] = componentsOfEntity;
            }

            componentsOfEntity.Add(component);
        }

        public void DeleteComponents<T>(Func<T, bool> predicate) where T : class, IComponent
        {
            var componentsToRemove = _flatComponentDict.Values.OfType<T>().Where(c => predicate.Invoke(c)).Select(c => c.Id).ToList();

            foreach (var componentId in componentsToRemove)
            {
                Delete(componentId);
            }
        }
    }
}
