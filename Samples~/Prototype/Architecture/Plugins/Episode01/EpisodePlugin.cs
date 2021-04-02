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

namespace BCKWorks.Prototype.Plugins.Episode01
{
    public class EpisodePlugin : IEcsRxPlugin
    {
        public const string EpisodeName = "Episode01";
        public const string SettingsName = EpisodeName + "Settings";
        public const string SettingsPath = "BCKWorks/" + Bootstrap.Name + "/Plugins/" + EpisodeName + "/Settings";
        public const string SystemNamespace = "BCKWorks." + Bootstrap.Name + ".Plugins." + EpisodeName + ".Systems";

        public string Name => EpisodeName;
        public Version Version => new Version(0, 1, 0);

        EpisodeInstaller episodeInstaller;

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
            var installers = Resources.FindObjectsOfTypeAll<ScriptableObjectInstaller>();
            var installer = installers.Where(x => x.name == SettingsName).FirstOrDefault();
            if (installer != null)
            {
                episodeInstaller = installer as EpisodeInstaller;
                if (episodeInstaller != null)
                {
                    episodeInstaller.Settings.Name = Name;
                    var nativeContainer = container.NativeContainer as DiContainer;
                    if (nativeContainer != null)
                    {
                        nativeContainer.BindInstance(episodeInstaller.Settings).IfNotBound();
                    }
                }
            }
        }

        void uninstallSettings(IDependencyContainer container)
        {
            if (episodeInstaller != null)
            {
                var nativeContainer = container.NativeContainer as DiContainer;
                if (nativeContainer != null)
                {
                    nativeContainer.Unbind<Settings>();
                }
            }
        }
    }
}