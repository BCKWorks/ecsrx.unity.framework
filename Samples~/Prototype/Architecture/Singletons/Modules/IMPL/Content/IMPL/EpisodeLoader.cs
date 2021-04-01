using EcsRx.Collections.Database;
using EcsRx.Events;
using EcsRx.Extensions;
using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using EcsRx.Unity.Extensions;
using EcsRx.Unity.Framework;

namespace BCKWorks.Prototype
{
    public class EpisodeLoader : IEpisodeLoader
	{
		public bool Ready { get; private set; }

		private readonly IEventSystem eventSystem;
		private readonly IEntityDatabase entityDatabase;
        private readonly IPluginLoader pluginLoader;
        const string baseSceneName = "Base";

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

		public IObservable<Unit> LoadBaseScenesAsync()
		{
			return Observable.FromCoroutine<Unit>((observer) => ensureLoadBaseScene(observer));
		}

		public IObservable<Unit> AddSceneAsync(string sceneName, bool activeScene = false)
		{
			return Observable.FromCoroutine<Unit>((observer) => ensureAddScene(observer, sceneName, activeScene));
		}

		public IObservable<Unit> RemoveSceneAsync(string sceneName)
		{
			return Observable.FromCoroutine<Unit>((observer) => ensureRemoveScene(observer, sceneName));
		}

		public IObservable<Unit> RemoveAllSceneAsync()
		{
			return Observable.FromCoroutine<Unit>((observer) => ensureRemoveAllScene(observer));
		}

		public IObservable<Unit> LoadEpisodeAsync(int id)
		{
			return Observable.FromCoroutine<Unit>((observer) => loadEpisode(observer, id));
		}

		public IObservable<Unit> UnloadEpisodeAsync()
		{
			return Observable.FromCoroutine<Unit>((observer) => unloadEpisode(observer));
		}

		IEnumerator ensureLoadBaseScene(IObserver<Unit> observer)
		{
			yield return ensureLoadSceneCO(baseSceneName);
			yield return ensureRemoveSceneCO(null);

			observer.OnNext(Unit.Default);
			observer.OnCompleted();
		}

		IEnumerator ensureAddScene(IObserver<Unit> observer, string sceneName, bool activeScene = false)
		{
			yield return ensureAddSceneCO(sceneName, activeScene);

			observer.OnNext(Unit.Default);
			observer.OnCompleted();
		}

		IEnumerator ensureRemoveScene(IObserver<Unit> observer, string sceneName)
		{
			yield return ensureRemoveSceneCO(sceneName);

			observer.OnNext(Unit.Default);
			observer.OnCompleted();
		}

		IEnumerator ensureRemoveAllScene(IObserver<Unit> observer)
		{
			yield return ensureRemoveAllSceneCO();

			observer.OnNext(Unit.Default);
			observer.OnCompleted();
		}

		IEnumerator ensureLoadSceneCO(string sceneName)
		{
			var scene = SceneManager.GetSceneByName(sceneName);
			if (scene.handle == 0)
			{
				var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
				while (!op.isDone)
					yield return null;
				scene = SceneManager.GetSceneByName(sceneName);
			}

			while (!scene.isLoaded)
				yield return null;

			yield return new WaitForEndOfFrame();
		}

		IEnumerator ensureAddSceneCO(string sceneName, bool activeScene = false)
		{
			var scene = SceneManager.GetSceneByName(sceneName);
			if (scene.handle == 0)
			{
				var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
				while (!op.isDone)
					yield return null;
				scene = SceneManager.GetSceneByName(sceneName);
			}

			while (!scene.isLoaded)
				yield return null;

			if (activeScene)
				SceneManager.SetActiveScene(scene);

			yield return new WaitForEndOfFrame();
		}

		IEnumerator ensureRemoveAllSceneCO()
		{
			List<Scene> removeScenes = new List<Scene>();
			int loadedSceneCount = SceneManager.sceneCount;
			for (int i = 0; i < loadedSceneCount; i++)
			{
				var loadedScene = SceneManager.GetSceneAt(i);
				if (loadedScene.handle != 0)
				{
					var loadedSceneName = loadedScene.name;
					if (loadedSceneName == baseSceneName)
						continue;

					var gameObjects = loadedScene.GetRootGameObjects();
					foreach (var gameObject in gameObjects)
					{
						var transforms = gameObject.GetComponentsInChildren<Transform>();
						foreach (var transform in transforms)
						{
							var go = transform.gameObject;
							var entity = go.GetEntity();
							if (entity != null)
							{
								entityDatabase.RemoveEntity(entity);
							}
						}
						GameObject.Destroy(gameObject);
					}
					yield return null;
					removeScenes.Add(loadedScene);
				}
			}

			foreach (var scene in removeScenes)
			{
				var op = SceneManager.UnloadSceneAsync(scene);
				while (!op.isDone)
					yield return null;
				while (scene.isLoaded)
					yield return null;
			}

			removeScenes.Clear();

			yield return new WaitForEndOfFrame();
		}

		IEnumerator ensureRemoveSceneCO(string sceneName)
		{
			List<Scene> removeScenes = new List<Scene>();
			int loadedSceneCount = SceneManager.sceneCount;
			for (int i = 0; i < loadedSceneCount; i++)
			{
				var loadedScene = SceneManager.GetSceneAt(i);
				if (loadedScene.handle != 0)
				{
					var loadedSceneName = loadedScene.name;
					if (loadedSceneName == baseSceneName)
						continue;
					if (sceneName != null && loadedSceneName != sceneName)
						continue;

					var gameObjects = loadedScene.GetRootGameObjects();
					foreach (var gameObject in gameObjects)
					{
						var transforms = gameObject.GetComponentsInChildren<Transform>();
						foreach (var transform in transforms)
						{
							var go = transform.gameObject;
							var entity = go.GetEntity();
							if (entity != null)
							{
								entityDatabase.RemoveEntity(entity);
							}
						}
						GameObject.Destroy(gameObject);
					}
					yield return null;
					removeScenes.Add(loadedScene);
				}
			}

			foreach (var scene in removeScenes)
			{
				var op = SceneManager.UnloadSceneAsync(scene);
				while (!op.isDone)
					yield return null;
				while (scene.isLoaded)
					yield return null;
			}

			removeScenes.Clear();

			yield return new WaitForEndOfFrame();
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

			eventSystem.Publish(new EpisodeLoadedEvent());
		}

		IEnumerator unloadEpisode(IObserver<Unit> observer)
		{
			yield return unloadEpisodeCO();

			observer.OnNext(Unit.Default);
			observer.OnCompleted();
		}

		IEnumerator unloadEpisodeCO()
		{
			// unload scenes except base scene
			yield return ensureRemoveSceneCO(null);

			// give some frametime
			yield return new WaitForEndOfFrame();

			unloadEpisodePlugin();
		}

		void loadEpisodePlugin(int id)
		{
			if (id == 1)
				pluginLoader.Load<BCKWorks.Prototype.Plugins.Episode01.EpisodePlugin>();
		}

		void unloadEpisodePlugin()
		{
			pluginLoader.Unload();
		}
	}
}
