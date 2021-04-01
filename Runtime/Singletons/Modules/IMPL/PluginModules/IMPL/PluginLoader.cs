using EcsRx.Extensions;
using EcsRx.Infrastructure.Plugins;
using EcsRx.Unity.Dependencies;
using EcsRx.Zenject;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Framework
{
    public class PluginLoader : IPluginLoader
    {
        public bool Ready { get; private set; }

        IEcsRxPlugin plugin;

        public IEnumerator Initialize()
        {
            yield return null;
            Ready = true;
        }

        public void Shutdown()
        {
            Unload();
        }

        public T Load<T>() where T : IEcsRxPlugin, new()
        {
            return load<T>();
        }


        T load<T>() where T : IEcsRxPlugin, new()
        {
            Unload();

            plugin = new T();

            var container = EcsRxApplicationBehaviour.Instance.Container;
            var systemExecutor = EcsRxApplicationBehaviour.Instance.SystemExecutor;
            plugin.SetupDependencies(container);
            plugin.GetSystemsForRegistration(container)
                .ForEachRun(x => systemExecutor.AddSystem(x));

            return (T)plugin;
        }

        public void Unload()
        {
            if (plugin != null)
            {
                var container = EcsRxApplicationBehaviour.Instance.Container;
                var systemExecutor = EcsRxApplicationBehaviour.Instance.SystemExecutor;
                plugin.GetSystemsForRegistration(container)
                    .ForEachRun(x => systemExecutor.RemoveSystem(x));
                plugin.UnsetupDependencies(container);
                plugin = null;
            }
        }
    }
}
