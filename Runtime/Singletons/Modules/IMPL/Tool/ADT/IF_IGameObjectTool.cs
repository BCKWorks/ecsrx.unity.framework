using EcsRx.Unity.Dependencies;
using UnityEngine;

namespace InterVR.IF.Modules
{
    public interface IF_IGameObjectTool : IF_IModule
    {
        GameObject InstantiateWithInit(GameObject prefab, Transform parent = null);
        GameObject InstantiateWithInit(IUnityInstantiator instantiator, GameObject prefab, Transform parent = null);
        void SetParentWithInit(GameObject go, Transform parent);
    }
}
