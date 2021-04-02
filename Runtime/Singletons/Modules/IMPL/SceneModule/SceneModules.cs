using EcsRx.Infrastructure.Extensions;
using System.Collections;
using EcsRx.Infrastructure.Dependencies;
using System.Collections.Generic;
using System;

namespace EcsRx.Unity.Framework
{
    public class SceneModules : IDependencyModule
    {
        List<Type> bindings = new List<Type>()
        {
            typeof(ISceneLoader),
        };

        public void Setup(IDependencyContainer container)
        {
            container.Bind<ISceneLoader, SceneLoader>();
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
