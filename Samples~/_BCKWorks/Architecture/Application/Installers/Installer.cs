using System;
using UnityEngine;
using Zenject;

namespace BCKWorks.Framework
{
    [Serializable]
    public class Settings
    {
        public string Name { get; set; }
        public int StartEpisode = 1;
        public int StartMission = 1;
    }

    [CreateAssetMenu(fileName = Bootstrap.SettingsName, menuName = Bootstrap.SettingsPath)]
    public class Installer : ScriptableObjectInstaller<Installer>
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