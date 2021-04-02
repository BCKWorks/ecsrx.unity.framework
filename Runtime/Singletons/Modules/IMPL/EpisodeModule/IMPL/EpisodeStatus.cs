using EcsRx.Collections.Database;
using EcsRx.Events;
using EcsRx.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Framework
{
    public class EpisodeStatus : IEpisodeStatus
	{
		public bool Ready { get; private set; }

        public int EpisodeId { get; private set; }

		private List<IDisposable> subscriptions = new List<IDisposable>();
        private readonly IEventSystem eventSystem;
		private readonly IEntityDatabase entityDatabase;
        private readonly IPluginLoader pluginLoader;

		public EpisodeStatus(IEventSystem eventSystem, IEntityDatabase entityDatabase,
			IPluginLoader pluginLoader)
		{
			this.eventSystem = eventSystem;
			this.entityDatabase = entityDatabase;
            this.pluginLoader = pluginLoader;

			EpisodeId = 0;

			eventSystem.Receive<EpisodeLoadedPluginEvent>().Subscribe(evt =>
			{
				EpisodeId = evt.Id;
			}).AddTo(subscriptions);
		}

		public IEnumerator Initialize()
		{
			yield return null;
			Ready = true;
		}

		public void Shutdown()
		{
			subscriptions.DisposeAll();
		}
	}
}
