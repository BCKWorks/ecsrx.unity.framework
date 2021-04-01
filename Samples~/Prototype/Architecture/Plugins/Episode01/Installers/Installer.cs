using System;
using UnityEngine;
using Zenject;

namespace BCKWorks.Prototype.Plugins.Episode01
{
    [Serializable]
    public class Settings
    {
        public string EpisodeSceneName = "Episode01";
    }

    [CreateAssetMenu(fileName = "Episode01Settings", menuName = "BCKWorksPrototype/Plugins/Episode01/Settings")]
    public class Installer : ScriptableObjectInstaller<Installer>
    {
#pragma warning disable 0649
        [SerializeField]
        Settings settings;
#pragma warning restore 0649

        public override void InstallBindings()
        {
            Container.BindInstance(settings).IfNotBound();
        }
    }
}