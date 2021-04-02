using System;
using UniRx;

namespace EcsRx.Unity.Framework
{
    public interface IEpisodeLoader : IModule
    {
        IObservable<Unit> LoadEpisodeAsync(int id, int missionId = 1);
        IObservable<Unit> UnloadEpisodeAsync();
    }
}
