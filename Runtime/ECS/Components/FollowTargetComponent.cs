using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Unity.MonoBehaviours;
using UnityEngine;

namespace EcsRx.Unity.Framework
{
    public class FollowTarget : IComponent
    {
        public Transform Target { get; set; }
        public bool MatchRotation { get; set; }
    }

    public class FollowTargetComponent : RegisterAsEntity
    {
        public Transform Target;
        public bool MatchRotation = true;

        public override void Convert(IEntity entity)
        {
            var component = new FollowTarget
            {
                Target = Target,
                MatchRotation = MatchRotation
            };

            entity.AddComponentSafe(component);
        }
    }
}