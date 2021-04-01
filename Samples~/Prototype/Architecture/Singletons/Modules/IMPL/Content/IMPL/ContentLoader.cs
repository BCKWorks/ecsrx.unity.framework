using EcsRx.Collections.Database;
using EcsRx.Events;
using EcsRx.Extensions;
using InterVR.IF.Extensions;
using InterVR.IF.Modules;
using Prototype.Defines;
using Prototype.Events;
using Prototype.Plugins.PrototypeContent;
using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype.Modules
{
    public class ContentLoader : IContentLoader
    {
        public bool Ready { get; private set; }

		const string baseSceneName = "Base";
        private readonly IEventSystem eventSystem;
        private readonly IEntityDatabase entityDatabase;
        private readonly IF_IContentPluginLoader contentPluginLoader;
        Scene baseScene;

		public ContentLoader(IEventSystem eventSystem, IEntityDatabase entityDatabase,
			IF_IContentPluginLoader contentPluginLoader)
        {
            this.eventSystem = eventSystem;
            this.entityDatabase = entityDatabase;
            this.contentPluginLoader = contentPluginLoader;
        }

        public IEnumerator Initialize()
        {
            yield return null;
            Ready = true;
        }

		public void Shutdown()
		{
		}

		public IObservable<Unit> EnsureLoadBaseAsync()
		{
			return Observable.FromCoroutine<Unit>((observer) => ensureLoadBase(observer));
		}

		IEnumerator ensureLoadBase(IObserver<Unit> observer)
		{
			yield return ensureLoadBaseCO();

			observer.OnNext(Unit.Default);
			observer.OnCompleted();
		}

		IEnumerator ensureLoadBaseCO()
		{
			baseScene = SceneManager.GetSceneByName(baseSceneName);
			if (baseScene.handle == 0)
			{
				var op = SceneManager.LoadSceneAsync(baseSceneName);
				while (!op.isDone)
					yield return null;
				baseScene = SceneManager.GetSceneByName(baseSceneName);
			}

			while (!baseScene.isLoaded)
				yield return null;
		}

		public IObservable<Unit> LoadContentAsync(ContentType type)
        {
			return Observable.FromCoroutine<Unit>((observer) => loadContentCO(observer, type));
		}

		IEnumerator loadContentCO(IObserver<Unit> observer, ContentType type)
		{
			eventSystem.Publish(new ContentLoadEvent() { });

			yield return ensureLoadBaseCO();

			// give some frametime
			yield return new WaitForEndOfFrame();

			// unload modules
			yield return unloadContentCO();

			// give some frametime
			yield return new WaitForEndOfFrame();

			// load plugin
			loadContentPlugin(type);

			// give some frametime
			yield return new WaitForEndOfFrame();

			// load target content scene
			string contentSceneName = type.ToString();
			var op = SceneManager.LoadSceneAsync(contentSceneName, LoadSceneMode.Additive);
			while (!op.isDone)
				yield return null;

			// check ensure loaded
			var contentScene = SceneManager.GetSceneByName(contentSceneName);
			while (!contentScene.isLoaded)
				yield return null;

			// give some frametime
			yield return new WaitForEndOfFrame();

			eventSystem.Publish(new ContentLoadedEvent() { });

			observer.OnNext(Unit.Default);
			observer.OnCompleted();
		}

		IEnumerator unloadContentCO()
		{
			eventSystem.Publish(new ContentUnloadEvent() { });

			int loadedSceneCount = SceneManager.sceneCount;
			for (int i = 0; i < loadedSceneCount; i++)
			{
				var loadedScene = SceneManager.GetSceneAt(i);
				if (loadedScene.name == baseSceneName)
					continue;
				if (loadedScene.handle != 0)
				{
					var loadedSceneName = loadedScene.name;
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
					var op = SceneManager.UnloadSceneAsync(loadedScene);
					while (!op.isDone)
						yield return null;
					while (loadedScene.isLoaded)
						yield return null;
				}
			}

			eventSystem.Publish(new ContentUnloadedEvent() { });

			unloadContentPlugin();
		}

		void loadContentPlugin(ContentType type)
		{
			unloadContentPlugin();

			if (type == ContentType.Prototype)
				contentPluginLoader.Load<PrototypeContentPlugin>();
		}

		void unloadContentPlugin()
		{
			contentPluginLoader.Unload();
		}
	}
}
