using EcsRx.Unity.Framework;
using System;
using UniRx;

namespace BCKWorks.Prototype
{
    public interface IEpisodeLoader : IModule
    {
        IObservable<Unit> LoadBaseScenesAsync();
        IObservable<Unit> LoadEpisodeAsync(int id);
        IObservable<Unit> UnloadEpisodeAsync();
        IObservable<Unit> AddSceneAsync(string sceneName, bool activeScene = false);
        IObservable<Unit> RemoveSceneAsync(string sceneName);
        IObservable<Unit> RemoveAllSceneAsync();
    }
}
