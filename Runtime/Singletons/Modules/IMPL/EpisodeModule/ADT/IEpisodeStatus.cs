using System;
using UniRx;

namespace EcsRx.Unity.Framework
{
    public interface IEpisodeStatus : IModule
    {
        int EpisodeId { get; }
    }
}
