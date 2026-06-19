// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using UnityEngine;

namespace UnityEditor
{
    /// *undocumented*
    internal class ListViewGUILayout
    {
        internal class GUILayoutedListViewGroup
        {
            public void ResetCursor()
            {
            }

            public void AddY()
            {
            }

            public void AddY(float y)
            {
            }
        }

        static public ListViewShared.ListViewElementsEnumerator ListView(ListViewState state, GUIStyle style, params GUILayoutOption[] options)
        {
            return ListViewGUI.ListView(state, style, options);
        }

        static public ListViewShared.ListViewElementsEnumerator ListView(ListViewState state, string dragTitle, GUIStyle style, params GUILayoutOption[] options)
        {
            return ListViewGUI.ListView(state, 0, dragTitle, style, options);
        }

        static public ListViewShared.ListViewElementsEnumerator ListView(ListViewState state, ListViewOptions lvOptions, GUIStyle style, params GUILayoutOption[] options)
        {
            return ListViewGUI.ListView(state, lvOptions, style, options);
        }

        static public ListViewShared.ListViewElementsEnumerator ListView(ListViewState state, ListViewOptions lvOptions, string dragTitle, GUIStyle style, params GUILayoutOption[] options)
        {
            return ListViewGUI.ListView(state, lvOptions, dragTitle, style, options);
        }

        static public bool MultiSelection(int prevSelected, int currSelected, ref int initialSelected, ref bool[] selectedItems)
        {
            return ListViewGUI.MultiSelection(prevSelected, currSelected, ref initialSelected, ref selectedItems);
        }

        static public bool HasMouseUp(Rect r)
        {
            return ListViewGUI.HasMouseUp(r);
        }

        static public bool HasMouseDown(Rect r)
        {
            return ListViewGUI.HasMouseDown(r);
        }

        static public bool HasMouseDown(Rect r, int button)
        {
            return ListViewGUI.HasMouseDown(r, button);
        }
    }
}
