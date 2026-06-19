#nullable enable

using System;
using UnityEditor;

namespace UnityEngine
{
    internal class DontDestroySceneObject : MonoBehaviour
    {
        public event Action<DontDestroySceneObject>? Destoryed;

        private void OnDestroy()
        {
            Destoryed?.Invoke(this);
        }
    }
}
