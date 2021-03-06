using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Plugins.Views.Components;
using EcsRx.Extensions;
using System.Collections.Generic;
using System;
using UniRx;
using EcsRx.Collections.Database;
using EcsRx.Unity.Extensions;
using UnityEngine;
using UniRx.Triggers;

namespace EcsRx.Unity.Framework.Systems
{
    public class DestroyAtStartSystem : ISetupSystem, ITeardownSystem
    {
        public IGroup Group => new Group(typeof(DestroyAtStart), typeof(ViewComponent));

        private Dictionary<IEntity, List<IDisposable>> subscriptionsPerEntity = new Dictionary<IEntity, List<IDisposable>>();
        private readonly IEntityDatabase entityDatabase;

        public DestroyAtStartSystem(IEntityDatabase entityDatabase)
        {
            this.entityDatabase = entityDatabase;
        }

        public void Setup(IEntity entity)
        {
            var subscriptions = new List<IDisposable>();
            subscriptionsPerEntity.Add(entity, subscriptions);

            var view = entity.GetGameObject();
            view.OnDestroyAsObservable().Subscribe(x =>
            {
                entityDatabase.RemoveEntity(entity);
            }).AddTo(subscriptions);
            GameObject.Destroy(view);
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