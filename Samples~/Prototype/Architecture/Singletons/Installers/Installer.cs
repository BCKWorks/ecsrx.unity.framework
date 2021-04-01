using System;
using UnityEngine;
using Zenject;

namespace BCKWorks.Prototype
{
    [Serializable]
    public class Settings
    {
        public string Name = "Prototype Installer";
        public int StartEpisode = 1;
        public int StartMission = 1;
    }

    [CreateAssetMenu(fileName = "PrototypeSettings", menuName = "BCKWorksPrototype/Settings")]
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