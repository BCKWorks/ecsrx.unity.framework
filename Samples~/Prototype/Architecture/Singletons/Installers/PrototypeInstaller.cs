using Prototype.Defines;
using System;
using UnityEngine;
using Zenject;

namespace Prototype.Installer
{
    [CreateAssetMenu(fileName = "PrototypeSettings", menuName = "InterVR/IF/Prototype/Settings")]
    public class PrototypeInstaller : ScriptableObjectInstaller<PrototypeInstaller>
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
            public string Name = "Prototype Installer";
            public ContentType StartContent;
        }
    }
}