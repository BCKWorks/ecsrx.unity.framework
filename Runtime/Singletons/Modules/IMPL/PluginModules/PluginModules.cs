using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EcsRx.Unity.Framework
{
    public class PluginModules : IDependencyModule
    {
        List<Type> bindings = new List<Type>()
        {
            typeof(IPluginLoader),
        };

        public void Setup(IDependencyContainer container)
        {
            container.Bind<IPluginLoader, PluginLoader>();
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
