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
        public const string SettingsName = "BCKWorks" + Name + "Settings";
        public const string SettingsPath = "BCKWorks/" + Name + "/Settings";

        List<IDisposable> subscriptions = new List<IDisposable>();

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
            var settings = Container.Resolve<Settings>();
            var pluginLoader = Container.Resolve<IPluginLoader>();

            this.Started = true;

            Observable.Interval(TimeSpan.FromSeconds(0.1f)).First().Subscribe(y =>
            {
                EventSystem.Publish(new EcsRxUnityFrameworkApplicationStartedEvent()
                {

                });
            });

            EventSystem.Receive<EcsRxUnityFrameworkApplicationStartedEvent>().Subscribe(evt =>
            {
                Debug.Log("Application Started");
                pluginLoader.Load<Plugins.Sample.Plugin>();
            }).AddTo(subscriptions);
        }

        void installSettings()
        {
            var installer = Resources.Load(SettingsName);
            if (installer != null)
            {
                var _installer = installer as Installer;
                if (_installer != null)
                {
                    _installer.Settings.Name = Name;
                    var nativeContainer = Container.NativeContainer as DiContainer;
                    if (nativeContainer != null)
                    {
                        nativeContainer.BindInstance(_installer.Settings).IfNotBound();
                    }
                }
            }
        }

        void uninstallSettings()
        {
            var nativeContainer = Container.NativeContainer as DiContainer;
            if (nativeContainer != null)
            {
                if (nativeContainer.HasBinding<Settings>())
                    nativeContainer.Unbind<Settings>();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            uninstallSettings();

            subscriptions.DisposeAll();

            Debug.Log("Application Shudowned");
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
