using System;
using UniRx;

namespace EcsRx.Unity.Framework
{
    public interface IEpisodeLoader : IModule
    {
        IObservable<Unit> LoadEpisodeAsync(int id);
        IObservable<Unit> UnloadEpisodeAsync();
    }
}
