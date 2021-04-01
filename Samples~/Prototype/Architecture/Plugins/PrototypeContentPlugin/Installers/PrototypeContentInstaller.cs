using System;
using UnityEngine;
using Zenject;

namespace Prototype.Plugins.PrototypeContent.Installer
{
    [CreateAssetMenu(fileName = "PrototypeContentSettings", menuName = "Prototype/PrototypeContent/Settings")]
    public class PrototypeContentInstaller : ScriptableObjectInstaller<PrototypeContentInstaller>
    {
#pragma warning disable 0649
        [SerializeField]
        Settings settings;
#pragma warning restore 0649

        public override void InstallBindings()
        {
            Container.BindInstance(settings).IfNotBound();
        }

        [Serializable]
        public class Settings
        {
            public string Name = "Prototype Content Installer";
        }
    }
}