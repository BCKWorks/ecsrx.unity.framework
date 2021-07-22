using EcsRx.Collections.Database;
using EcsRx.Events;
using EcsRx.Extensions;
using System;
using System.Collections;
using UniRx;
using UnityEngine;
using System.Collections.Generic;

namespace EcsRx.Unity.Framework.Modules.Scene
{
    public class SceneLoader : ISceneLoader
	{
		public bool Ready { get; private set; }

		private readonly IEventSystem eventSystem;
		private readonly IEntityDatabase entityDatabase;
        const string baseSceneName = "Base";

		public SceneLoader(IEventSystem eventSystem, IEntityDatabase entityDatabase)
		{
			this.eventSystem = eventSystem;
			this.entityDatabase = entityDatabase;
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
			var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
			if (scene.handle == 0)
			{
				var op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
				while (!op.isDone)
					yield return null;
				scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
			}

			while (!scene.isLoaded)
				yield return null;

			yield return new WaitForEndOfFrame();
		}

		IEnumerator ensureAddSceneCO(string sceneName, bool activeScene = false)
		{
			var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
			if (scene.handle == 0)
			{
				var op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
				while (!op.isDone)
					yield return null;
				scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
			}

			while (!scene.isLoaded)
				yield return null;

			if (activeScene)
				UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);

			yield return new WaitForEndOfFrame();
		}

		IEnumerator ensureRemoveAllSceneCO()
		{
			List<UnityEngine.SceneManagement.Scene> removeScenes = new List<UnityEngine.SceneManagement.Scene>();
			int loadedSceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
			for (int i = 0; i < loadedSceneCount; i++)
			{
				var loadedScene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
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
				var op = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
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
			List<UnityEngine.SceneManagement.Scene> removeScenes = new List<UnityEngine.SceneManagement.Scene>();
			int loadedSceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
			for (int i = 0; i < loadedSceneCount; i++)
			{
				var loadedScene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
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
				var op = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
				while (!op.isDone)
					yield return null;
				while (scene.isLoaded)
					yield return null;
			}

			removeScenes.Clear();

			yield return new WaitForEndOfFrame();
		}
	}
}
