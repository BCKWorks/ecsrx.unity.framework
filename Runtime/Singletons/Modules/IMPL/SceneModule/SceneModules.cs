using EcsRx.Infrastructure.Extensions;
using System.Collections;
using EcsRx.Infrastructure.Dependencies;

namespace EcsRx.Unity.Framework
{
    public class SceneModules : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            container.Bind<ISceneLoader, SceneLoader>();
        }

        public IEnumerator Initialize(IDependencyContainer container)
        {
            yield return container.Resolve<ISceneLoader>().Initialize();
        }

        public void Shutdown(IDependencyContainer container)
        {
            container.Resolve<ISceneLoader>().Shutdown();
            container.Unbind<ISceneLoader>();
        }
    }
}
