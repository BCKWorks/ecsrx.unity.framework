using System;
using System.Collections.Generic;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Infrastructure.Plugins;
using EcsRx.Systems;

namespace Prototype.Plugins.PrototypeContent
{
    public class PrototypeContentPlugin : IEcsRxPlugin
    {
        public string Name => "Prototype Content Plugin";
        public Version Version => new Version(0, 1, 0);

        const string systemNamespace = "Prototype.Plugins.PrototypeContent." + "Systems";

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