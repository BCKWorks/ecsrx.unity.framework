using System;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Framework
{
    [CreateAssetMenu(fileName = "EcsRxUnityFrameworkSettings", menuName = "BCKWorks/EcsRxUnityFramework/Settings")]
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

        [Serializable]
        public class Settings
        {
            public string Name = "BCKWorks EcsRxUnityFramework Installer";
        }
    }
}