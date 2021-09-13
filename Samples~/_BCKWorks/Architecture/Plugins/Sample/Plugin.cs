using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Infrastructure.Plugins;
using EcsRx.Systems;
using EcsRx.Zenject.Extensions;
using UnityEngine;
using Zenject;

namespace BCKWorks.Framework.Plugins.Sample
{
    public class Plugin : IEcsRxPlugin
    {
        public const string PluginName = "Sample";
        public const string SettingsName = "BCKWorks" + PluginName + "PluginSettings";
        public const string SettingsPath = "BCKWorks/" + Bootstrap.Name + "/Plugins/" + PluginName + "/Settings";
        public const string SystemNamespace = "BCKWorks.Framework" + ".Plugins." + PluginName + ".Systems";

        public string Name => PluginName;
        public Version Version => new Version(0, 1, 0);

        public void SetupDependencies(IDependencyContainer container)
        {
            installSettings(container);
            container.BindApplicableSystems(SystemNamespace);
        }

        public void UnsetupDependencies(IDependencyContainer container)
        {
            container.UnbindApplicableSystems(SystemNamespace);
            uninstallSettings(container);
        }

        public IEnumerable<ISystem> GetSystemsForRegistration(IDependencyContainer container)
        {
            return container.ResolveApplicableSystems(SystemNamespace);
        }

        void installSettings(IDependencyContainer container)
        {
            var installer = Resources.Load(SettingsName);
            if (installer != null)
            {
                var _installer = installer as Installer;
                if (_installer != null)
                {
                    _installer.Settings.Name = Name;
                    var nativeContainer = container.NativeContainer as DiContainer;
                    if (nativeContainer != null)
                    {
                        nativeContainer.BindInstance(_installer.Settings).IfNotBound();
                    }
                }
            }
        }

        void uninstallSettings(IDependencyContainer container)
        {
            var nativeContainer = container.NativeContainer as DiContainer;
            if (nativeContainer != null)
            {
                if (nativeContainer.HasBinding<Settings>())
                    nativeContainer.Unbind<Settings>();
            }
        }
    }
}