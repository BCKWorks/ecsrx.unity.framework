using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using System.Collections;

namespace InterVR.IF.Modules
{
    public class IF_PluginModules : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            container.Bind<IF_IContentPluginLoader, IF_ContentPluginLoader>();
        }

        public IEnumerator Initialize(IDependencyContainer container)
        {
            yield return container.Resolve<IF_IContentPluginLoader>().Initialize();
        }

        public void Shutdown(IDependencyContainer container)
        {
            container.Resolve<IF_IContentPluginLoader>().Shutdown();
            container.Unbind<IF_IContentPluginLoader>();
        }
    }
}
