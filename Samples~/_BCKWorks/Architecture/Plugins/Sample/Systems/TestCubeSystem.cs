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
using EcsRx.Unity.Framework.Modules.Scene;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Entities;
using BCKWorks.Framework.Components;

namespace BCKWorks.Framework.Plugins.Sample.Systems
{
    public class TestCubeSystem : IReactToGroupSystem
    {
        List<IDisposable> subscriptions = new List<IDisposable>();
        private readonly IEntityDatabase entityDatabase;
        private readonly IEventSystem eventSystem;
        private readonly Settings settings;
        private readonly ISceneLoader sceneLoader;

        public IGroup Group => new Group(typeof(TestCube));

        public TestCubeSystem(IEntityDatabase entityDatabase,
            IEventSystem eventSystem,
            Settings settings,
            ISceneLoader sceneLoader)
        {
            this.entityDatabase = entityDatabase;
            this.eventSystem = eventSystem;
            this.settings = settings;
            this.sceneLoader = sceneLoader;
        }

        public IObservable<IObservableGroup> ReactToGroup(IObservableGroup observableGroup)
        {
            return Observable.EveryUpdate().Select(x => observableGroup);
        }

        public void Process(IEntity entity)
        {
            var testCube = entity.GetComponent<TestCube>();
            testCube.Timer += Time.deltaTime;
            if (testCube.Timer >= testCube.Interval)
            {
                Debug.Log("Test Cube has reached time out");
                testCube.Timer = 0;
            }
        }
    }
}