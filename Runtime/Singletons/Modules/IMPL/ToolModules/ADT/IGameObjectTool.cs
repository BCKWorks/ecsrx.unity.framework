using EcsRx.Unity.Dependencies;
using UnityEngine;

namespace EcsRx.Unity.Framework
{
    public interface IGameObjectTool : IModule
    {
        GameObject InstantiateWithInit(GameObject prefab, Transform parent = null);
        GameObject InstantiateWithInit(IUnityInstantiator instantiator, GameObject prefab, Transform parent = null);
        void SetParentWithInit(GameObject go, Transform parent);
    }
}
