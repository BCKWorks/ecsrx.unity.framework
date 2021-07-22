using EcsRx.Infrastructure.Plugins;
using EcsRx.Unity.Dependencies;
using System;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Framework.Modules.Plugin
{
    public interface IPluginLoader : IModule
    {
        T Load<T>() where T: IEcsRxPlugin, new();
        void Unload(IEcsRxPlugin plugin, bool remove = true);
        void Unload<T>() where T : IEcsRxPlugin;
        void UnloadAll();
    }
}
