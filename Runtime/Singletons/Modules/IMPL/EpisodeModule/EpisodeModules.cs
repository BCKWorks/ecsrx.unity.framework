using EcsRx.Infrastructure.Extensions;
using System.Collections;
using EcsRx.Infrastructure.Dependencies;

namespace EcsRx.Unity.Framework
{
    public class EpisodeModules : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            container.Bind<IEpisodeLoader, EpisodeLoader>();
        }

        public IEnumerator Initialize(IDependencyContainer container)
        {
            yield return container.Resolve<IEpisodeLoader>().Initialize();
        }

        public void Shutdown(IDependencyContainer container)
        {
            container.Resolve<IEpisodeLoader>().Shutdown();
            container.Unbind<IEpisodeLoader>();
        }
    }
}
