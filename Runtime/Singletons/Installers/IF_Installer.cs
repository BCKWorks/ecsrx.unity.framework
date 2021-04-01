using System;
using UnityEngine;
using Zenject;

namespace InterVR.IF.Installer
{
    [CreateAssetMenu(fileName = "IF_Settings", menuName = "InterVR/IF/Settings")]
    public class IF_Installer : ScriptableObjectInstaller<IF_Installer>
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
            public string Name = "IF Installer";
        }
    }
}