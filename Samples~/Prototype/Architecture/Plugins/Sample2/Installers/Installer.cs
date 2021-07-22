using System;
using UnityEngine;
using Zenject;

namespace BCKWorks.Prototype.Plugins.Sample2
{
    [Serializable]
    public class Settings
    {
        public string Name { get; set; }
    }

    [CreateAssetMenu(fileName = Plugin.SettingsName, menuName = Plugin.SettingsPath)]
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