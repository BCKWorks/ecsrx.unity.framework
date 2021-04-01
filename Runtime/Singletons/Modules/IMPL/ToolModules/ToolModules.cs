using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using System.Collections;

namespace EcsRx.Unity.Framework
{
    public class ToolModules : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            container.Bind<IGameObjectTool, GameObjectTool>();
        }

        public IEnumerator Initialize(IDependencyContainer container)
        {
            yield return container.Resolve<IGameObjectTool>().Initialize();
        }

        public void Shutdown(IDependencyContainer container)
        {
            container.Resolve<IGameObjectTool>().Shutdown();
            container.Unbind<IGameObjectTool>();
        }
    }
}
