using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Plugins.Views.Components;
using EcsRx.Extensions;
using EcsRx.Events;
using System.Collections.Generic;
using System;
using UniRx;
using EcsRx.Unity.Extensions;
using UniRx.Triggers;
using EcsRx.Collections.Database;
using Prototype.Components;
using Prototype.Events;

namespace Prototype.Systems
{
    public class ContentInfoSystem : ISetupSystem, ITeardownSystem
    {
        public IGroup Group => new Group(typeof(ContentInfo), typeof(ViewComponent));

        private List<IDisposable> subscriptions = new List<IDisposable>();
        private readonly IEntityDatabase entityDatabase;
        private readonly IEventSystem eventSystem;

        public ContentInfoSystem(IEntityDatabase entityDatabase, IEventSystem eventSystem)
        {
            this.entityDatabase = entityDatabase;
            this.eventSystem = eventSystem;
        }

        public void Setup(IEntity entity)
        {
            var view = entity.GetGameObject();
            view.OnDestroyAsObservable().Subscribe(x =>
            {
                entityDatabase.RemoveEntity(entity);
            }).AddTo(subscriptions);

            eventSystem.Receive<ContentLoadedEvent>().Subscribe(evt =>
            {
                var info = entity.GetComponent<ContentInfo>();
                eventSystem.Publish(new ContentSetupEvent() { Info = info });
            }).AddTo(subscriptions);
        }

        public void Teardown(IEntity entity)
        {
            subscriptions.DisposeAll();
            subscriptions.Clear();
        }
    }
}