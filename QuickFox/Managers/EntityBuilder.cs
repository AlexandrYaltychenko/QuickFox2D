using System;
using QuickFox.Components;
using QuickFox.Rendering;

namespace QuickFox.Managers
{
    public class EntityBuilder<T> where T : IEntity, new()
    {
        private readonly IEntityManager _entityManager;
        private readonly IComponentManager _componentManager;
        private T _entity;
        private string _sceneIdToAttach;

        public EntityBuilder(IEntityManager entityManager,
                             IComponentManager componentManager)
        {
            _entityManager = entityManager;
            _componentManager = componentManager;
            _entity = _entityManager.CreateEntity<T>();
        }

        public EntityBuilder<T> AddComponent<K>(Action<K> postCreate) where K : class, IComponent
        {
            var component = _componentManager.CreateComponent<K>(_entity.Id);
            postCreate.Invoke(component);
            return this;
        }

        public EntityBuilder<T> AttachToScene(Scene scene)
        {
            _sceneIdToAttach = scene.Id;
            return this;
        }

        public EntityBuilder<T> AttachToScene(string sceneId)
        {
            _sceneIdToAttach = sceneId;
            return this;
        }

        public T Create()
        {
            _entity.SceneId = _sceneIdToAttach;
            return _entity;
        }
    }
}
