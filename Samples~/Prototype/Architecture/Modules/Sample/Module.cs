using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BCKWorks.Prototype.Modules.Sample
{
    public class Module : IDependencyModule
    {
        readonly List<Type> bindings = new List<Type>()
        {
            typeof(ISampleInterface),
        };

        public void Setup(IDependencyContainer container)
        {
            container.Bind<ISampleInterface, SampleInterface>();
        }

        public IEnumerator Initialize(IDependencyContainer container)
        {
            foreach (var bind in bindings)
            {
                yield return ((ISampleInterface)container.Resolve(bind)).Initialize();
            }
        }

        public void Shutdown(IDependencyContainer container)
        {
            foreach (var bind in bindings)
            {
                ((ISampleInterface)container.Resolve(bind)).Shutdown();
                container.Unbind(bind);
            }
        }
    }
}
