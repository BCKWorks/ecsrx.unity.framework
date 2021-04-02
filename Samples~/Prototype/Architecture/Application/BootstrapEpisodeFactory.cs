using EcsRx.Extensions;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Unity.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace BCKWorks.Prototype
{
    public partial class Bootstrap
    {
        void episodeFactory()
        {
            var pluginLoader = Container.Resolve<IPluginLoader>();
            if (pluginLoader == null)
                return;

            EventSystem.Receive<EpisodeLoadPluginEvent>().Subscribe(evt =>
            {
                if (evt.ID == 1)
                {
                    pluginLoader.Load<Plugins.Episode01.EpisodePlugin>();
                }
            }).AddTo(subscriptions);

            EventSystem.Receive<EpisodeUnloadPluginEvent>().Subscribe(evt =>
            {
                pluginLoader.Unload();
            }).AddTo(subscriptions);
        }
    }
}
