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

namespace BCKWorks.Prototype.Plugins.Episode01.Systems
{
    public class EpisodeBootstrapSystem : IManualSystem
    {
        List<IDisposable> subscriptions = new List<IDisposable>();
        private readonly IEntityDatabase entityDatabase;
        private readonly IEventSystem eventSystem;
        private readonly Settings settings;
        private readonly IEpisodeLoader episodeLoader;
        private readonly ISceneLoader sceneLoader;

        public IGroup Group => new EmptyGroup();

        public EpisodeBootstrapSystem(IEntityDatabase entityDatabase,
            IEventSystem eventSystem,
            Settings settings,
            IEpisodeLoader episodeLoader,
            ISceneLoader sceneLoader)
        {
            this.entityDatabase = entityDatabase;
            this.eventSystem = eventSystem;
            this.settings = settings;
            this.episodeLoader = episodeLoader;
            this.sceneLoader = sceneLoader;
        }

        public void StartSystem(IObservableGroup observableGroup)
        {
            Debug.Log($"Episode Bootstrap System {settings.Name}");

            // from IContentLoader.LoadContentAsync
            eventSystem.Receive<EpisodeLoadedPluginEvent>().Subscribe(evt =>
            {
                sceneLoader.RemoveAllSceneAsync().Subscribe(_ =>
                {
                    sceneLoader.AddSceneAsync(settings.EpisodeSceneName, true).Subscribe(z =>
                    {
                        initEpisodeAsync().Subscribe(_ =>
                        {
                            eventSystem.Publish(new EpisodeReadyEvent());
                        }).AddTo(subscriptions);
                    }).AddTo(subscriptions);
                }).AddTo(subscriptions);
            }).AddTo(subscriptions);
        }

        public void StopSystem(IObservableGroup observableGroup)
        {
            subscriptions.DisposeAll();
        }

        IObservable<Unit> initEpisodeAsync()
        {
            return Observable.FromCoroutine<Unit>((observer) => initEpisodeCO(observer));
        }

        IEnumerator initEpisodeCO(IObserver<Unit> observer)
        {
            yield return new WaitForEndOfFrame();

            observer.OnNext(Unit.Default);
            observer.OnCompleted();
        }
    }
}