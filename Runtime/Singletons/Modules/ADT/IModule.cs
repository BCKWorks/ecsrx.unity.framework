using EcsRx.Unity.Dependencies;
using System.Collections;
using UnityEngine;

namespace EcsRx.Unity.Framework
{
    public interface IModule
    {
        bool Ready { get; }
        IEnumerator Initialize();
        void Shutdown();
    }
}
