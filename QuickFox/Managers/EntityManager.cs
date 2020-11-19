using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using QuickFox.Components;
using QuickFox.Managers;
using QuickFox.UI;

namespace QuickFox.Engine
{
    public class EntityManager : IEntityManager
    {
        private IDictionary<string, IEntity> _entities;

        public EntityManager()
        {
            _entities = new ConcurrentDictionary<string, IEntity>();
        }

        public T CreateEntity<T>() where T : IEntity, new()
        {
            var entity = new T();
            _entities.Add(entity.Id, entity);
            return entity;
        }


        public bool AttachEntity<T>(T entity) where T : IEntity
        {
            if (_entities.ContainsKey(entity.Id))
                return false;

            _entities.Add(entity.Id, entity);
            return true;
        }

        public IList<IEntity> GetAllEntitiesForScene(string sceneId)
        {
            return _entities.Values.Where(entity => entity.SceneId == sceneId).ToList();
        }

        public IList<T> GetAllEntitiesForScene<T>(string sceneId) where T : IEntity
        {
            return _entities.Values.Where(entity => entity.SceneId == sceneId).OfType<T>().ToList();
        }

        public IEntity GetEntity(string entityId)
        {
            return _entities[entityId];
        }

        public void DeleteEntity(string entityId)
        {
            _entities.Remove(entityId);
        }

        public IList<T> GetAllGameEntitiesForScene<T>(string sceneId) where T : GameEntity
        {
            return _entities.Values.OfType<T>().Where(e => e.SceneId == sceneId).ToList();
        }

        public IList<T> GetAllUIEntitiesForScene<T>(string sceneId) where T : UIEntity
        {
            return _entities.Values.OfType<T>().Where(e => e.SceneId == sceneId).ToList();
        }
    }
}
