using System;
using System.Collections.Generic;
using QuickFox.Components;
using QuickFox.UI;

namespace QuickFox.Managers
{
    public interface IEntityManager
    {
        bool AttachEntity<T>(T entity) where T: IEntity;

        T CreateEntity<T>() where T : IEntity, new();

        IEntity GetEntity(string entityId);

        IList<IEntity> GetAllEntitiesForScene(string sceneId);

        IList<T> GetAllEntitiesForScene<T>(string sceneId) where T : IEntity;

        IList<T> GetAllGameEntitiesForScene<T>(string sceneId) where T : GameEntity;

        IList<T> GetAllUIEntitiesForScene<T>(string sceneId) where T : UIEntity;

        void DeleteEntity(string entityId);
    }
}
