using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Groups.Observable;
using EcsRx.Collections.Database;
using EcsRx.Extensions;
using System.Collections.Generic;
using System;
using UniRx;
using EcsRx.Events;

namespace BCKWorks.Prototype.Plugins.Episode01
{
    public class EpisodeBootstrapSystem : IManualSystem
    {
        List<IDisposable> subscriptions = new List<IDisposable>();
        private readonly IEntityDatabase entityDatabase;
        private readonly IEventSystem eventSystem;
        private readonly Settings settings;
        private readonly IEpisodeLoader episodeLoader;

        public IGroup Group => new EmptyGroup();

        public EpisodeBootstrapSystem(IEntityDatabase entityDatabase,
            IEventSystem eventSystem,
            Settings settings,
            IEpisodeLoader episodeLoader)
        {
            this.entityDatabase = entityDatabase;
            this.eventSystem = eventSystem;
            this.settings = settings;
            this.episodeLoader = episodeLoader;
        }

        public void StartSystem(IObservableGroup observableGroup)
        {
            // from IContentLoader.LoadContentAsync
            eventSystem.Receive<EpisodeLoadedEvent>().Subscribe(evt =>
            {
                episodeLoader.RemoveAllSceneAsync().Subscribe(_ =>
                {
                    //episodeLoader.AddSceneAsync("").Subscribe(x =>
                    //{
                        episodeLoader.AddSceneAsync("Episode01", true).Subscribe(z =>
                        {
                            //initContentAsync().Subscribe(_ =>
                            //{
                            //    eventSystem.Publish(new EpisodeReadyEvent());
                            //}).AddTo(subscriptions);
                        }).AddTo(subscriptions);
                    //}).AddTo(subscriptions);
                }).AddTo(subscriptions);
            }).AddTo(subscriptions);
        }

        public void StopSystem(IObservableGroup observableGroup)
        {
            //contentInterface.UnloadQuestInfo();
            //contentInterface.UnloadMissionInfo();
            //contentInterface.UnloadContentInfo();

            subscriptions.DisposeAll();
        }

        //IObservable<Unit> initContentAsync()
        //{
        //    return Observable.FromCoroutine<Unit>((observer) => initContent(observer));
        //}

        //IEnumerator initContent(IObserver<Unit> observer)
        //{
        //    //var playerGravityEntity = vrInterface.GetPlayerControllerEntity();
        //    //if (playerGravityEntity != null)
        //    //{
        //    //    var playerGravity = playerGravityEntity.GetUnityComponent<PlayerGravity>();
        //    //    playerGravity.GravityEnabled = true;
        //    //}

        //    //yield return initDatabase();

        //    //contentInterface.LoadContentInfo(ContentType.Content1);

        //    observer.OnNext(Unit.Default);
        //    observer.OnCompleted();
        //}

        //IEnumerator initDatabase()
        //{
        //    yield return database.SetDataboxObject(DatabaseType.Mission, contentSettings.DBMissions);
        //    yield return database.SetDataboxObject(DatabaseType.Quest, contentSettings.DBQuests);
        //}
    }
}