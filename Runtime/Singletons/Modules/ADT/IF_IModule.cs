using EcsRx.Unity.Dependencies;
using System.Collections;
using UnityEngine;

namespace InterVR.IF.Modules
{
    public interface IF_IModule
    {
        bool Ready { get; }
        IEnumerator Initialize();
        void Shutdown();
    }
}
