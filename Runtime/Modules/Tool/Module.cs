using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EcsRx.Unity.Framework.Modules.Tool
{
    public class Module : IDependencyModule
    {
        readonly List<Type> bindings = new List<Type>()
        {
            typeof(IGameObjectTool),
        };

        public void Setup(IDependencyContainer container)
        {
            container.Bind<IGameObjectTool, GameObjectTool>();
        }

        public IEnumerator Initialize(IDependencyContainer container)
        {
            foreach (var bind in bindings)
            {
                yield return ((IModule)container.Resolve(bind)).Initialize();
            }
        }

        public void Shutdown(IDependencyContainer container)
        {
            foreach (var bind in bindings)
            {
                ((IModule)container.Resolve(bind)).Shutdown();
                container.Unbind(bind);
            }
        }
    }
}
