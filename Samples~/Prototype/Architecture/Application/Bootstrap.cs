using EcsRx.Extensions;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Unity.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace BCKWorks.Prototype
{
    public partial class Bootstrap : EcsRxUnityFrameworkApplicationBehaviour
    {
        public const string Name = "Prototype";
        public const string SettingsName = Name + "Settings";
        public const string SettingsPath = "BCKWorks/" + Name + "/Settings";

        List<IDisposable> subscriptions = new List<IDisposable>();
        Installer bootstrapInstaller;

        protected override void BindSystems()
        {
            base.BindSystems();

            installSettings();
        }

        protected override void LoadModules()
        {
            base.LoadModules();
        }

        protected override void ApplicationStarted()
        {
            episodeFactory();

            var settings = Container.Resolve<Settings>();
            var episodeLoader = Container.Resolve<IEpisodeLoader>();
            var sceneLoader = Container.Resolve<ISceneLoader>();
            sceneLoader.LoadBaseScenesAsync().Subscribe(x =>
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

                episodeLoader.LoadEpisodeAsync(settings.StartEpisode).Subscribe(x =>
                {
                });
            }).AddTo(subscriptions);

            ///
            /// Everytime Plugin Initialized
            /// 
            EventSystem.Receive<EpisodeReadyEvent>().Subscribe(evt =>
            {
                Debug.Log("Episode Ready Event");
                EventSystem.Publish(new EpisodeStartedEvent());
            }).AddTo(subscriptions);
        }

        void installSettings()
        {
            var installers = Resources.FindObjectsOfTypeAll<ScriptableObjectInstaller>();
            var installer = installers.Where(x => x.name == SettingsName).FirstOrDefault();
            if (installer != null)
            {
                bootstrapInstaller = installer as Installer;
                if (bootstrapInstaller != null)
                {
                    bootstrapInstaller.Settings.Name = Name;
                    var nativeContainer = Container.NativeContainer as DiContainer;
                    if (nativeContainer != null)
                    {
                        nativeContainer.BindInstance(bootstrapInstaller.Settings).IfNotBound();
                    }
                }
            }
        }

        void uninstallSettings()
        {
            if (bootstrapInstaller != null)
            {
                var nativeContainer = Container.NativeContainer as DiContainer;
                if (nativeContainer != null)
                {
                    nativeContainer.Unbind<Settings>();
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            uninstallSettings();

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
