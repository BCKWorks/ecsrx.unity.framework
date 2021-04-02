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

		public IObservable<Unit> LoadEpisodeAsync(int id)
		{
			return Observable.FromCoroutine<Unit>((observer) => loadEpisode(observer, id));
		}

		public IObservable<Unit> UnloadEpisodeAsync()
		{
			return Observable.FromCoroutine<Unit>((observer) => unloadEpisode(observer));
		}

		IEnumerator loadEpisode(IObserver<Unit> observer, int id)
		{
			yield return unloadEpisodeCO();

			yield return loadEpisodeCO(id);

			observer.OnNext(Unit.Default);
			observer.OnCompleted();
		}

		IEnumerator loadEpisodeCO(int id)
		{
			yield return unloadEpisodeCO();

			// load plugin
			loadEpisodePlugin(id);

			// give some frametime
			yield return new WaitForEndOfFrame();

			eventSystem.Publish(new EpisodeLoadedPluginEvent());
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

		void loadEpisodePlugin(int id)
		{
			eventSystem.Publish(new EpisodeLoadPluginEvent()
			{
				ID = id
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
