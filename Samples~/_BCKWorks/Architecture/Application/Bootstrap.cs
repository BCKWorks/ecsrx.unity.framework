using EcsRx.Extensions;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Unity.Framework;
using EcsRx.Unity.Framework.Modules.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace BCKWorks.Framework
{
    public partial class Bootstrap : EcsRxUnityFrameworkApplicationBehaviour
    {
        public const string Name = "Framework";
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

            Container.LoadModule<Libraries.Sample.Module>();
        }

        protected override void ApplicationStarted()
        {
            var settings = Container.Resolve<Settings>();

            // scene first component registry start
            this.Started = true;

            Observable.Interval(TimeSpan.FromSeconds(0.1f)).First().Subscribe(y =>
            {
                EventSystem.Publish(new EcsRxUnityFrameworkApplicationStartedEvent()
                {

                });
            });

            EventSystem.Receive<EcsRxUnityFrameworkApplicationStartedEvent>().Subscribe(evt =>
            {
                // system ready
                Debug.Log($"{Name} Application Started");

                var pluginLoader = Container.Resolve<IPluginLoader>();
                pluginLoader.Load<Plugins.Sample.Plugin>();

                var sampleInterface = Container.Resolve<Libraries.Sample.ISampleInterface>();
                if (sampleInterface != null)
                {
                    sampleInterface.Print();
                }
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

            Debug.Log($"{Name} Application Shutdowned");
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
