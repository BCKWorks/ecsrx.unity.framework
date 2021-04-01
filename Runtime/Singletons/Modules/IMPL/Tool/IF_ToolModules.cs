using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using System.Collections;

namespace InterVR.IF.Modules
{
    public class IF_ToolModules : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            container.Bind<IF_IGameObjectTool, IF_GameObjectTool>();
        }

        public IEnumerator Initialize(IDependencyContainer container)
        {
            yield return container.Resolve<IF_IGameObjectTool>().Initialize();
        }

        public void Shutdown(IDependencyContainer container)
        {
            container.Resolve<IF_IGameObjectTool>().Shutdown();
            container.Unbind<IF_IGameObjectTool>();
        }
    }
}
