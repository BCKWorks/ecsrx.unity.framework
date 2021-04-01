using EcsRx.Unity.Dependencies;
using System.Collections;
using UnityEngine;

namespace InterVR.IF.Modules
{
    public class IF_GameObjectTool : IF_IGameObjectTool
    {
        public bool Ready { get; private set; }

        public IEnumerator Initialize()
        {
            yield return null;
            Ready = true;
        }

        public void Shutdown()
        {
        }

        public GameObject InstantiateWithInit(GameObject prefab, Transform parent = null)
        {
            var instance = GameObject.Instantiate(prefab) as GameObject;
            instance.name = instance.name.Replace("(Clone)", "");
            if (parent != null)
                instance.transform.SetParent(parent);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localEulerAngles = Vector3.zero;
            instance.transform.localScale = Vector3.one;

            return instance;
        }

        public GameObject InstantiateWithInit(IUnityInstantiator instantiator, GameObject prefab, Transform parent = null)
        {
            var instance = instantiator.InstantiatePrefab(prefab);
            instance.name = instance.name.Replace("(Clone)", "");
            if (parent != null)
                instance.transform.SetParent(parent);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localEulerAngles = Vector3.zero;
            instance.transform.localScale = Vector3.one;

            return instance;
        }

        public void SetParentWithInit(GameObject go, Transform parent)
        {
            go.transform.SetParent(parent);
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
        }
    }
}
