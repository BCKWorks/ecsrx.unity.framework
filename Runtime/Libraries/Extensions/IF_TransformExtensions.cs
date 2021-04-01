using UnityEngine;

namespace InterVR.IF.Extensions
{
    public static class IF_TransformExtensions
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