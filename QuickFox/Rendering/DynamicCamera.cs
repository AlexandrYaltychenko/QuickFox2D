using System;
using System.Linq;
using QuickFox.Components;
using QuickFox.Managers;

namespace QuickFox.Rendering
{
    public class DynamicCamera : ICamera
    {
        private readonly IEntityManager _entityManager;
        private readonly IComponentManager _componentManager;

        public string EntityId { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; } = 1f;

        public DynamicCamera(IEntityManager entityManager, IComponentManager componentManager)
        {
            _entityManager = entityManager;
            _componentManager = componentManager;
        }

        public void Update()
        {
            if (string.IsNullOrWhiteSpace(EntityId))
            {
                ResetToDefaults();
            }
            else
            {
                var entity = _entityManager.GetEntity(EntityId);
                if (entity == null)
                {
                    ResetToDefaults();
                }
                else
                {
                    var positionComponent = _componentManager.GetComponentsForEntity(EntityId).OfType<PositionComponent>().FirstOrDefault();
                    if (positionComponent != null)
                    {
                        X = positionComponent.X;
                        Y = positionComponent.Y;
                    }
                    else
                    {
                        ResetToDefaults();
                    }
                }
            }
        }

        private void ResetToDefaults()
        {
            X = 0;
            Y = 0;
        }

        public void ZoomTo(float coef)
        {
            Z = coef;
        }

        public void ZoomBy(float coef)
        {
            Z *= coef;
        }
    }
}
