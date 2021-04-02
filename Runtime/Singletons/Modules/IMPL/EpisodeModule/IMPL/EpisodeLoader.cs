using EcsRx.Collections.Database;
using EcsRx.Events;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Framework
{
    public class EpisodeLoader : IEpisodeLoader
	{
		public bool Ready { get; private set; }

		private readonly IEventSystem eventSystem;
		private readonly IEntityDatabase entityDatabase;
        private readonly IPluginLoader pluginLoader;

		public EpisodeLoader(IEventSystem eventSystem, IEntityDatabase entityDatabase,
			IPluginLoader pluginLoader)
		{
			this.eventSystem = eventSystem;
			this.entityDatabase = entityDatabase;
            this.pluginLoader = pluginLoader;
        }

		public IEnumerator Initialize()
		{
			yield return null;
			Ready = true;
		}

		public void Shutdown()
		{
		}

		public IObservable<Unit> LoadEpisodeAsync(int id, int missionId = 1)
		{
			return Observable.FromCoroutine<Unit>((observer) => loadEpisode(observer, id, missionId));
		}

		public IObservable<Unit> UnloadEpisodeAsync()
		{
			return Observable.FromCoroutine<Unit>((observer) => unloadEpisode(observer));
		}

		IEnumerator loadEpisode(IObserver<Unit> observer, int id, int missionId = 1)
		{
			yield return unloadEpisodeCO();

			yield return loadEpisodeCO(id, missionId);

			observer.OnNext(Unit.Default);
			observer.OnCompleted();
		}

		IEnumerator loadEpisodeCO(int id, int missionId = 1)
		{
			yield return unloadEpisodeCO();

			// load plugin
			loadEpisodePlugin(id, missionId);

			// give some frametime
			yield return new WaitForEndOfFrame();

			eventSystem.Publish(new EpisodeLoadedPluginEvent()
			{
				Id = id,
				MissionId = missionId
			});
		}

		IEnumerator unloadEpisode(IObserver<Unit> observer)
		{
			yield return unloadEpisodeCO();

			observer.OnNext(Unit.Default);
			observer.OnCompleted();
		}

		IEnumerator unloadEpisodeCO()
		{
			unloadEpisodePlugin();

			// give some frametime
			yield return new WaitForEndOfFrame();
		}

		void loadEpisodePlugin(int id, int missionId = 1)
		{
			eventSystem.Publish(new EpisodeLoadPluginEvent()
			{
				Id = id,
				MissionId = missionId
			});
		}

		void unloadEpisodePlugin()
		{
			eventSystem.Publish(new EpisodeUnloadPluginEvent()
			{

			});
		}
	}
}
