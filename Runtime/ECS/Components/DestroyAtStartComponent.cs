using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Unity.MonoBehaviours;
using UnityEngine;

namespace EcsRx.Unity.Framework
{
    public class DestroyAtStart : IComponent
    {
    }

    public class DestroyAtStartComponent : RegisterAsEntity
    {
        public override void Convert(IEntity entity)
        {
            if (!isActiveAndEnabled)
            {
                Destroy(this);
                return;
            }

            var component = new DestroyAtStart();

            entity.AddComponentSafe(component);

            Destroy(this);
        }
    }
}