using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Extensions;
using System.Collections.Generic;
using System;
using EcsRx.Events;
using UniRx;
using EcsRx.Collections.Database;
using UnityEngine;

namespace BCKWorks.Prototype.Systems
{
    public class ContentSystem : ISetupSystem, ITeardownSystem
    {
        public IGroup Group => new Group(typeof(EpisodeInfo));

        private List<IDisposable> subscriptions = new List<IDisposable>();
        private readonly IEntityDatabase entityDatabase;
        private readonly IEventSystem eventSystem;
        private readonly Settings settings;

        public ContentSystem(IEntityDatabase entityDatabase, IEventSystem eventSystem,
            Settings settings)
        {
            this.entityDatabase = entityDatabase;
            this.eventSystem = eventSystem;
            this.settings = settings;
        }

        public void Setup(IEntity entity)
        {
            var contentInfo = entity.GetComponent<EpisodeInfo>();

            eventSystem.Receive<EpisodeStartedEvent>().Subscribe(evt =>
            {
                //Debug.Log($"Episode Started: {contentInfo.Type}");
                //if (!contentInterface.LoadMissionInfo(settings.StartMission, true))
                //{
                //    Debug.LogWarning($"There is no start mission ({settings.StartMission}) skip content");
                //    eventSystem.Publish(new ContentCompleteEvent());
                //}
            }).AddTo(subscriptions);

            //eventSystem.Receive<MissionCompleteEvent>().Subscribe(evt =>
            //{
            //    if (!contentInterface.NextMissionInfo())
            //    {
            //        eventSystem.Publish(new ContentCompleteEvent());
            //    }
            //}).AddTo(subscriptions);
        }

        public void Teardown(IEntity entity)
        {
            subscriptions.DisposeAll();
        }
    }
}