using EcsRx.Extensions;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Unity.Framework;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace BCKWorks.Prototype
{
    public class PrototypeBootstrap : EcsRxUnityFrameworkApplicationBehaviour
    {
        List<IDisposable> subscriptions = new List<IDisposable>();

        protected override void BindSystems()
        {
            base.BindSystems();
        }

        protected override void LoadModules()
        {
            base.LoadModules();

            Container.LoadModule<EpisodeModules>();
        }

        protected override void ApplicationStarted()
        {
            var settings = Container.Resolve<Settings>();
            var contentLoader = Container.Resolve<IEpisodeLoader>();
            contentLoader.LoadBaseScenesAsync().Subscribe(x =>
            {
                this.Started = true;

                Observable.Interval(TimeSpan.FromSeconds(0.1f)).First().Subscribe(y =>
                {
                    EventSystem.Publish(new EcsRxUnityFrameworkApplicationStartedEvent()
                    {
                    });
                });
            }).AddTo(subscriptions);

            EventSystem.Receive<EcsRxUnityFrameworkApplicationStartedEvent>().Subscribe(evt =>
            {
                Debug.Log("Application Started");

                contentLoader.LoadEpisodeAsync(settings.StartEpisode).Subscribe(x =>
                {
                });
            }).AddTo(subscriptions);

            ///
            /// Everytime Plugin Initialized
            /// 
            EventSystem.Receive<EpisodeReadyEvent>().Subscribe(evt =>
            {
                Debug.Log("Content Ready Event");
                EventSystem.Publish(new EpisodeStartedEvent());
            }).AddTo(subscriptions);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            subscriptions.DisposeAll();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause == false)
            {
            }
        }

        private void OnApplicationFocus(bool focus)
        {

        }
    }
}
