using System;
using UnityEngine;

namespace UnityEditor
{
    internal struct ListViewElement
    {
        public int row;
        public int column;
        public Rect position;
    }

    internal sealed class SplitterState
    {
        internal readonly float[] relativeSizes;
        internal readonly float[] minSizes;
        internal readonly float[] maxSizes;

        private SplitterState(float[] relativeSizes, float[] minSizes, float[] maxSizes)
        {
            this.relativeSizes = relativeSizes ?? Array.Empty<float>();
            this.minSizes = minSizes ?? Array.Empty<float>();
            this.maxSizes = maxSizes ?? Array.Empty<float>();
        }

        public static SplitterState FromRelative(float[] relativeSizes, float[] minSizes, float[] maxSizes)
        {
            return new SplitterState(relativeSizes, minSizes, maxSizes);
        }
    }

    internal static class SplitterGUILayout
    {
        public static void BeginVerticalSplit(SplitterState state)
        {
            GUILayout.BeginVertical();
        }

        public static void EndVerticalSplit()
        {
            GUILayout.EndVertical();
        }
    }
}

namespace UnityEditor.Networking.PlayerConnection
{
    internal struct EditorConnectionTarget
    {
    }
}

namespace UnityEngine.Scripting
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    internal sealed class RequiredByNativeCodeAttribute : Attribute
    {
    }
}
