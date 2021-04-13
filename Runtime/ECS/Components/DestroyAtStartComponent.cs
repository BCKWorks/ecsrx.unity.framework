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
        public override bool Convert(IEntity entity)
        {
            if (!base.Convert(entity))
                return false;

            var component = new DestroyAtStart();

            entity.AddComponentSafe(component);

            Destroy(this);

            return true;
        }
    }
}