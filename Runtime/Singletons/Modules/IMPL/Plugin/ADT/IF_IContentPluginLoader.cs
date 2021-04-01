using EcsRx.Infrastructure.Plugins;
using EcsRx.Unity.Dependencies;
using System;
using UniRx;
using UnityEngine;

namespace InterVR.IF.Modules
{
    public interface IF_IContentPluginLoader : IF_IModule
    {
        T Load<T>() where T: IEcsRxPlugin, new();
        void Unload();
    }
}
