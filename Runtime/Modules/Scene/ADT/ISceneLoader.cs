using System;
using UniRx;

namespace EcsRx.Unity.Framework.Modules.Scene
{
    public interface ISceneLoader : IModule
    {
        IObservable<Unit> LoadBaseScenesAsync();
        IObservable<Unit> AddSceneAsync(string sceneName, bool activeScene = false);
        IObservable<Unit> RemoveSceneAsync(string sceneName);
        IObservable<Unit> RemoveAllSceneAsync();
    }
}
