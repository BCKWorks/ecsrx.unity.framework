using EcsRx.Zenject;
using EcsRx.Infrastructure.Extensions;
using UnityEngine;
using InterVR.IF.Installer;
using Prototype.Installer;
using InterVR.IF.Modules;
using System.Collections;
using EcsRx.Infrastructure.Dependencies;

namespace Prototype.Modules
{
    public class ContentModules : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            container.Bind<IContentLoader, ContentLoader>();
        }

        public IEnumerator Initialize(IDependencyContainer container)
        {
            yield return container.Resolve<IContentLoader>().Initialize();
        }

        public void Shutdown(IDependencyContainer container)
        {
            container.Resolve<IContentLoader>().Shutdown();
            container.Unbind<IContentLoader>();
        }
    }
}
