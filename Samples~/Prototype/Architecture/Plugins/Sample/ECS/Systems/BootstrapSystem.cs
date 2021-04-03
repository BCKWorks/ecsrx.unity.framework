using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Groups.Observable;
using EcsRx.Collections.Database;
using EcsRx.Extensions;
using System.Collections.Generic;
using System;
using UniRx;
using EcsRx.Events;
using UnityEngine;
using EcsRx.Unity.Framework;
using System.Collections;

namespace BCKWorks.Prototype.Plugins.Sample.Systems
{
    public class BootstrapSystem : IManualSystem
    {
        List<IDisposable> subscriptions = new List<IDisposable>();
        private readonly IEntityDatabase entityDatabase;
        private readonly IEventSystem eventSystem;
        private readonly Settings settings;
        private readonly ISceneLoader sceneLoader;

        public IGroup Group => new EmptyGroup();

        public BootstrapSystem(IEntityDatabase entityDatabase,
            IEventSystem eventSystem,
            Settings settings,
            ISceneLoader sceneLoader)
        {
            this.entityDatabase = entityDatabase;
            this.eventSystem = eventSystem;
            this.settings = settings;
            this.sceneLoader = sceneLoader;
        }

        public void StartSystem(IObservableGroup observableGroup)
        {
            Debug.Log($"Sample Bootstrap System {settings.Name}");
        }

        public void StopSystem(IObservableGroup observableGroup)
        {
            Debug.Log("Unload Sample Bootstrap System");

            subscriptions.DisposeAll();
        }
    }
}