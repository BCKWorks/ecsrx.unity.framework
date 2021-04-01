using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using System.Collections;

namespace EcsRx.Unity.Framework
{
    public class PluginModules : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            container.Bind<IPluginLoader, PluginLoader>();
        }

        public IEnumerator Initialize(IDependencyContainer container)
        {
            yield return container.Resolve<IPluginLoader>().Initialize();
        }

        public void Shutdown(IDependencyContainer container)
        {
            container.Resolve<IPluginLoader>().Shutdown();
            container.Unbind<IPluginLoader>();
        }
    }
}
