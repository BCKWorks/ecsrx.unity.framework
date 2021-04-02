using System;
using UnityEngine;
using Zenject;

namespace BCKWorks.Prototype.Plugins.Episode01
{
    [Serializable]
    public class Settings
    {
        public string Name { get; set; }
        public string EpisodeSceneName = EpisodePlugin.EpisodeName;
    }

    [CreateAssetMenu(fileName = EpisodePlugin.SettingsName, menuName = EpisodePlugin.SettingsPath)]
    public class EpisodeInstaller : ScriptableObjectInstaller<EpisodeInstaller>
    {
#pragma warning disable 0649
        [SerializeField]
        public Settings Settings;
#pragma warning restore 0649

        public override void InstallBindings()
        {
            Container.BindInstance(Settings).IfNotBound();
        }
    }
}