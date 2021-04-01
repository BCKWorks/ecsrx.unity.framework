using System;
using System.Collections.Generic;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Infrastructure.Plugins;
using EcsRx.Systems;

namespace BCKWorks.Prototype.Plugins.Episode01
{
    public class EpisodePlugin : IEcsRxPlugin
    {
        public string Name => "Episode01 Plugin";
        public Version Version => new Version(0, 1, 0);

        const string systemNamespace = "BCKWorks.Prototype.Plugins.Episode01." + "Systems";

        public void SetupDependencies(IDependencyContainer container)
        {
            container.BindApplicableSystems(systemNamespace);
        }

        public void UnsetupDependencies(IDependencyContainer container)
        {
            container.UnbindApplicableSystems(systemNamespace);
        }

        public IEnumerable<ISystem> GetSystemsForRegistration(IDependencyContainer container)
        {
            return container.ResolveApplicableSystems(systemNamespace);
        }
    }
}