using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Plugins.Views.Components;
using EcsRx.Extensions;
using System.Collections.Generic;
using System;
using EcsRx.Events;
using UniRx;
using EcsRx.Collections.Database;
using EcsRx.Unity.Extensions;
using UnityEngine;
using UniRx.Triggers;
using System.Linq;
using EcsRx.Unity.MonoBehaviours;

namespace EcsRx.Unity.Framework.Systems
{
    public class DestroyAtStartSystem : ISetupSystem, ITeardownSystem
    {
        public IGroup Group => new Group(typeof(DestroyAtStart), typeof(ViewComponent));

        private Dictionary<IEntity, List<IDisposable>> subscriptionsPerEntity = new Dictionary<IEntity, List<IDisposable>>();
        private readonly IEntityDatabase entityDatabase;
        private readonly IEventSystem eventSystem;

        public DestroyAtStartSystem(IEntityDatabase entityDatabase, IEventSystem eventSystem)
        {
            this.entityDatabase = entityDatabase;
            this.eventSystem = eventSystem;
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

        private void EditorGUICallbacks_OnDrawGizmosSelectedEventCallback()
        {
        }
    }
}