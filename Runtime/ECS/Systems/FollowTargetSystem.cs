using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Plugins.Views.Components;
using EcsRx.Extensions;
using System.Collections.Generic;
using System;
using UniRx;
using EcsRx.Unity.Extensions;

namespace EcsRx.Unity.Framework.Systems
{
    public class FollowTargetSystem : ISetupSystem, ITeardownSystem
    {
        public IGroup Group => new Group(typeof(FollowTarget), typeof(ViewComponent));

        private Dictionary<IEntity, List<IDisposable>> subscriptionsPerEntity = new Dictionary<IEntity, List<IDisposable>>();

        public void Setup(IEntity entity)
        {
            var subscriptions = new List<IDisposable>();
            subscriptionsPerEntity.Add(entity, subscriptions);

            var view = entity.GetGameObject();
            var transform = view.transform;
            var followTarget = entity.GetComponent<FollowTarget>();
            Observable.EveryUpdate().Subscribe(x =>
            {
                if (followTarget.Target != null)
                {
                    var target = followTarget.Target;
                    transform.position = target.position;
                    if (followTarget.MatchRotation)
                        transform.rotation = target.rotation;
                }
            }).AddTo(subscriptions);
        }

        public void Teardown(IEntity entity)
        {
            if (subscriptionsPerEntity.TryGetValue(entity, out List<IDisposable> subscriptions))
            {
                subscriptions.DisposeAll();
                subscriptions.Clear();
                subscriptionsPerEntity.Remove(entity);
            }
        }
    }
}