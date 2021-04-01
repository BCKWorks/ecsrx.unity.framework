using UnityEngine;

namespace EcsRx.Unity.Framework
{
    public static class TransformExtensions
    {
        public static void ChangeLayersRecursively(this Transform trans, string name)
        {
            trans.gameObject.layer = LayerMask.NameToLayer(name);
            foreach (Transform child in trans)
            {
                child.ChangeLayersRecursively(name);
            }
        }
    }
}