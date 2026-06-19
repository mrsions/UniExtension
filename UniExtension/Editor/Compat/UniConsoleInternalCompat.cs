using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace UniExtension.Editor.Internal
{
    internal static class EditorReflectionUtility
    {
        private static readonly Assembly s_EditorAssembly = typeof(EditorWindow).Assembly;

        internal static Type GetEditorType(string fullName)
        {
            return s_EditorAssembly.GetType(fullName);
        }

        internal static MethodInfo GetMethod(Type type, string name, params Type[] parameterTypes)
        {
            if (type == null)
                return null;
            return type.GetMethod(name, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, parameterTypes, null);
        }
    }

    internal static class UniEditorGUI
    {
        private static readonly MethodInfo s_ToolbarSearchField =
            EditorReflectionUtility.GetMethod(typeof(EditorGUI), "ToolbarSearchField", typeof(Rect), typeof(string), typeof(bool));

        private static readonly MethodInfo s_DrawWithTextSelection =
            EditorReflectionUtility.GetMethod(typeof(GUIStyle), "DrawWithTextSelection", typeof(Rect), typeof(GUIContent),
                typeof(bool), typeof(bool), typeof(int), typeof(int), typeof(bool), typeof(Color));

        internal static string ToolbarSearchField(Rect position, string text, bool showWithPopup)
        {
            if (s_ToolbarSearchField != null)
                return (string)s_ToolbarSearchField.Invoke(null, new object[] { position, text, showWithPopup });

            return EditorGUI.TextField(position, text, EditorStyles.toolbarSearchField);
        }

        internal static void DrawWithTextSelection(GUIStyle style, Rect position, GUIContent content, bool isActive,
            bool hasKeyboardFocus, int start, int end, bool drawAsComposition, Color selectionColor)
        {
            if (style == null)
                return;

            if (s_DrawWithTextSelection != null)
            {
                s_DrawWithTextSelection.Invoke(style, new object[]
                {
                    position, content, isActive, hasKeyboardFocus, start, end, drawAsComposition, selectionColor
                });
                return;
            }

            style.Draw(position, content, isActive, hasKeyboardFocus, false, false);
        }
    }

    internal static class UniEditorGUIUtility
    {
        private static readonly MethodInfo s_LoadIcon =
            EditorReflectionUtility.GetMethod(typeof(EditorGUIUtility), "LoadIcon", typeof(string));
        private static readonly MethodInfo s_GetHyperlinkColorForSkin =
            EditorReflectionUtility.GetMethod(typeof(EditorGUIUtility), "GetHyperlinkColorForSkin");

        internal static Texture2D LoadIcon(string name)
        {
            if (s_LoadIcon != null)
                return s_LoadIcon.Invoke(null, new object[] { name }) as Texture2D;

            GUIContent content = EditorGUIUtility.IconContent(name);
            return content?.image as Texture2D;
        }

        internal static Color GetHyperlinkColorForSkin()
        {
            if (s_GetHyperlinkColorForSkin != null)
            {
                try
                {
                    object value = s_GetHyperlinkColorForSkin.Invoke(null, null);
                    if (value is Color color)
                        return color;
                    if (value is Color32 color32)
                        return color32;
                }
                catch
                {
                    // 현재 Unity 버전에 따라 내부 반환 형식이 달라도 콘솔 렌더링이 깨지지 않도록 기본 색상으로 대체한다.
                }
            }

            return EditorGUIUtility.isProSkin ? new Color(0.36f, 0.55f, 1f) : new Color(0.06f, 0.3f, 0.8f);
        }

        internal static float SingleLineHeight => EditorGUIUtility.singleLineHeight;
    }

    internal static class UniEditorGUILayout
    {
        internal static float LabelFloatMaxW
        {
            get
            {
                float width = EditorGUIUtility.labelWidth;
                return width > 0 ? width : 150f;
            }
        }

        internal static bool DropDownToggle(ref bool clicked, GUIContent content, GUIStyle style)
        {
            bool pressed = GUILayout.Button(content, style);
            clicked = pressed;
            return pressed;
        }
    }

    internal static class UniEditorStyles
    {
        internal static GUIStyle ToolbarDropDownToggle
        {
            get
            {
                GUIStyle style = EditorStyles.toolbarDropDown;
                return style ?? EditorStyles.toolbarButton;
            }
        }
    }

    internal static class UniGUIContent
    {
        private static readonly GUIContent s_Temp = new GUIContent();

        internal static GUIContent Temp(string text)
        {
            s_Temp.text = text;
            s_Temp.tooltip = null;
            s_Temp.image = null;
            return s_Temp;
        }
    }

    internal static class UniEventCommandNames
    {
        internal const string Copy = "Copy";
        internal const string Find = "Find";
    }

    internal static class UniGUIClip
    {
        private static readonly Type s_Type = EditorReflectionUtility.GetEditorType("UnityEditor.GUIClip");
        private static readonly PropertyInfo s_VisibleRect =
            s_Type?.GetProperty("visibleRect", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        internal static Rect VisibleRect
        {
            get
            {
                if (s_VisibleRect != null)
                    return (Rect)s_VisibleRect.GetValue(null);

                return new Rect(0, 0, Screen.width > 0 ? Screen.width : 1f, Screen.height > 0 ? Screen.height : 1f);
            }
        }
    }

    internal static class UniGUILayoutUtility
    {
        private static readonly MethodInfo s_EndLayoutGroup =
            EditorReflectionUtility.GetMethod(typeof(GUILayoutUtility), "EndLayoutGroup");

        internal static void EndLayoutGroup()
        {
            s_EndLayoutGroup?.Invoke(null, null);
        }
    }

    internal class UniDragAndDropDelay
    {
        public Vector2 mouseDownPosition;

        public bool CanStartDrag()
        {
            Vector2 delta = Event.current.mousePosition - mouseDownPosition;
            return delta.sqrMagnitude > 9f;
        }
    }

    internal static class ConsoleWindowUtility
    {
        private static readonly Type s_Type = EditorReflectionUtility.GetEditorType("UnityEditor.ConsoleWindowUtility");
        private static readonly MethodInfo s_InternalCallLogsHaveChanged =
            EditorReflectionUtility.GetMethod(s_Type, "Internal_CallLogsHaveChanged");

        internal static void Internal_CallLogsHaveChanged()
        {
            s_InternalCallLogsHaveChanged?.Invoke(null, null);
        }
    }

    internal class LogEntry
    {
        public string message;
        public string file;
        public int mode;
        public int instanceID;
        public int globalLineIndex;
        public int callstackTextStartUTF8;
        public int callstackTextStartUTF16;
#if UNITY_6000_5_OR_NEWER
        public UnityEngine.EntityId entityId;
#endif
    }

    internal struct LogEntryStruct
    {
    }

    internal static class LogEntries
    {
        private static readonly Type s_Type =
            EditorReflectionUtility.GetEditorType("UnityEditor.LogEntries")
            ?? EditorReflectionUtility.GetEditorType("UnityEditorInternal.LogEntries");
        private static readonly Type s_LogEntryType =
            EditorReflectionUtility.GetEditorType("UnityEditor.LogEntry")
            ?? EditorReflectionUtility.GetEditorType("UnityEditorInternal.LogEntry");

        private static readonly FieldInfo s_ConsoleFlagsField =
            s_Type?.GetField("consoleFlags", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        private static readonly PropertyInfo s_ConsoleFlagsProperty =
            s_Type?.GetProperty("consoleFlags", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly MethodInfo s_SetConsoleFlag =
            EditorReflectionUtility.GetMethod(s_Type, "SetConsoleFlag", typeof(int), typeof(bool));
        private static readonly MethodInfo s_GetFilteringText =
            EditorReflectionUtility.GetMethod(s_Type, "GetFilteringText");
        private static readonly MethodInfo s_SetFilteringText =
            EditorReflectionUtility.GetMethod(s_Type, "SetFilteringText", typeof(string));
        private static readonly MethodInfo s_GetEntryRowIndexHinted =
            EditorReflectionUtility.GetMethod(s_Type, "GetEntryRowIndex", typeof(int), typeof(int));
        private static readonly MethodInfo s_GetEntryRowIndex =
            EditorReflectionUtility.GetMethod(s_Type, "GetEntryRowIndex", typeof(int));
        private static readonly MethodInfo s_GetCount =
            EditorReflectionUtility.GetMethod(s_Type, "GetCount");
        private static readonly MethodInfo s_Clear =
            EditorReflectionUtility.GetMethod(s_Type, "Clear");
        private static readonly MethodInfo s_GetCountsByType =
            EditorReflectionUtility.GetMethod(s_Type, "GetCountsByType", typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(int).MakeByRefType());
        private static readonly MethodInfo s_StartGettingEntries =
            EditorReflectionUtility.GetMethod(s_Type, "StartGettingEntries");
        private static readonly MethodInfo s_EndGettingEntries =
            EditorReflectionUtility.GetMethod(s_Type, "EndGettingEntries");
        private static readonly MethodInfo s_GetEntryInternal =
            EditorReflectionUtility.GetMethod(s_Type, "GetEntryInternal", typeof(int), s_LogEntryType);
        private static readonly MethodInfo s_GetLinesAndModeFromEntryInternal =
            EditorReflectionUtility.GetMethod(s_Type, "GetLinesAndModeFromEntryInternal", typeof(int), typeof(int), typeof(int).MakeByRefType(), typeof(string).MakeByRefType());
        private static readonly MethodInfo s_GetEntryCount =
            EditorReflectionUtility.GetMethod(s_Type, "GetEntryCount", typeof(int));
        private static readonly MethodInfo s_RowGotDoubleClicked =
            EditorReflectionUtility.GetMethod(s_Type, "RowGotDoubleClicked", typeof(int));
        private static readonly MethodInfo s_AddMessageWithDoubleClickCallback =
            EditorReflectionUtility.GetMethod(s_Type, "AddMessageWithDoubleClickCallback", s_LogEntryType);
        private static readonly MethodInfo s_AddMessagesImpl =
            FindAddMessagesImpl();
        private static readonly MethodInfo s_GetCallstackFormattedSignatureInternal =
            EditorReflectionUtility.GetMethod(s_Type, "GetCallstackFormattedSignatureInternal", typeof(MethodInfo));

        internal static int consoleFlags
        {
            get
            {
                if (s_ConsoleFlagsField != null)
                    return (int)s_ConsoleFlagsField.GetValue(null);
                if (s_ConsoleFlagsProperty != null)
                    return (int)s_ConsoleFlagsProperty.GetValue(null);
                return 0;
            }
        }

        internal static void SetConsoleFlag(int flag, bool value)
        {
            s_SetConsoleFlag?.Invoke(null, new object[] { flag, value });
        }

        internal static string GetFilteringText()
        {
            if (s_GetFilteringText == null)
                return string.Empty;
            return (string)s_GetFilteringText.Invoke(null, null);
        }

        internal static void SetFilteringText(string text)
        {
            s_SetFilteringText?.Invoke(null, new object[] { text });
        }

        internal static int GetEntryRowIndex(int globalLineIndex, int oldRow)
        {
            if (s_GetEntryRowIndexHinted == null)
                return -1;
            return (int)s_GetEntryRowIndexHinted.Invoke(null, new object[] { globalLineIndex, oldRow });
        }

        internal static int GetEntryRowIndex(int globalLineIndex)
        {
            if (s_GetEntryRowIndex == null)
                return -1;
            return (int)s_GetEntryRowIndex.Invoke(null, new object[] { globalLineIndex });
        }

        internal static int GetCount()
        {
            if (s_GetCount == null)
                return 0;
            return (int)s_GetCount.Invoke(null, null);
        }

        internal static void Clear()
        {
            s_Clear?.Invoke(null, null);
        }

        internal static void GetCountsByType(ref int errorCount, ref int warningCount, ref int logCount)
        {
            if (s_GetCountsByType == null)
            {
                errorCount = 0;
                warningCount = 0;
                logCount = 0;
                return;
            }

            object[] args = { errorCount, warningCount, logCount };
            s_GetCountsByType.Invoke(null, args);
            errorCount = (int)args[0];
            warningCount = (int)args[1];
            logCount = (int)args[2];
        }

        internal static int StartGettingEntries()
        {
            if (s_StartGettingEntries == null)
                return 0;
            return (int)s_StartGettingEntries.Invoke(null, null);
        }

        internal static void EndGettingEntries()
        {
            s_EndGettingEntries?.Invoke(null, null);
        }

        internal static void GetEntryInternal(int row, LogEntry outputEntry)
        {
            if (outputEntry == null)
                return;

            if (s_GetEntryInternal == null || s_LogEntryType == null)
                return;

            object internalEntry = Activator.CreateInstance(s_LogEntryType);
            s_GetEntryInternal.Invoke(null, new object[] { row, internalEntry });
            CopyFromInternal(internalEntry, outputEntry);
        }

        internal static void GetLinesAndModeFromEntryInternal(int row, int lineCount, ref int mode, ref string outString)
        {
            if (s_GetLinesAndModeFromEntryInternal == null)
            {
                mode = 0;
                outString = string.Empty;
                return;
            }

            object[] args = { row, lineCount, mode, outString };
            s_GetLinesAndModeFromEntryInternal.Invoke(null, args);
            mode = (int)args[2];
            outString = (string)args[3];
        }

        internal static int GetEntryCount(int row)
        {
            if (s_GetEntryCount == null)
                return 0;
            return (int)s_GetEntryCount.Invoke(null, new object[] { row });
        }

        internal static void RowGotDoubleClicked(int row)
        {
            s_RowGotDoubleClicked?.Invoke(null, new object[] { row });
        }

        internal static void AddMessageWithDoubleClickCallback(LogEntry entry)
        {
            if (entry == null || s_AddMessageWithDoubleClickCallback == null || s_LogEntryType == null)
                return;

            object internalEntry = Activator.CreateInstance(s_LogEntryType);
            CopyToInternal(entry, internalEntry);
            s_AddMessageWithDoubleClickCallback.Invoke(null, new object[] { internalEntry });
        }

        internal static unsafe void AddMessagesImpl(void* messages, int count)
        {
            if (s_AddMessagesImpl == null)
                return;
            ParameterInfo[] parameters = s_AddMessagesImpl.GetParameters();
            if (parameters.Length != 2)
                return;

            Type firstParamType = parameters[0].ParameterType;
            object firstArg;
            if (firstParamType == typeof(IntPtr))
            {
                firstArg = (IntPtr)messages;
            }
            else if (firstParamType.IsPointer)
            {
                firstArg = Pointer.Box(messages, firstParamType);
            }
            else
            {
                return;
            }

            s_AddMessagesImpl.Invoke(null, new object[] { firstArg, count });
        }

        internal static string GetCallstackFormattedSignatureInternal(MethodInfo method)
        {
            if (s_GetCallstackFormattedSignatureInternal == null)
                return null;
            return (string)s_GetCallstackFormattedSignatureInternal.Invoke(null, new object[] { method });
        }
#if UNITY_6000_5_OR_NEWER
        private static object GetFieldOrPropertyValue(Type type, object instance, string name)
        {
            FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
                return field.GetValue(instance);

            PropertyInfo prop = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null)
                return prop.GetValue(instance);

            return null;
        }
#endif

        private static void CopyFromInternal(object internalEntry, LogEntry outputEntry)
        {
            if (internalEntry == null || outputEntry == null)
                return;

            Type type = internalEntry.GetType();
            outputEntry.message = GetFieldOrProperty<string>(type, internalEntry, "message");
            outputEntry.file = GetFieldOrProperty<string>(type, internalEntry, "file");
            outputEntry.mode = GetFieldOrProperty<int>(type, internalEntry, "mode");

#if UNITY_6000_5_OR_NEWER
            object instanceIDObj = GetFieldOrPropertyValue(type, internalEntry, "instanceID") ?? GetFieldOrPropertyValue(type, internalEntry, "entityId");
            if (instanceIDObj is int id)
            {
                outputEntry.instanceID = id;
            }
            else if (instanceIDObj != null && instanceIDObj.GetType().Name == "EntityId")
            {
                var entityId = (UnityEngine.EntityId)instanceIDObj;
                outputEntry.entityId = entityId;
                outputEntry.instanceID = 0; // Not used in Unity 6000+
            }
            else
            {
                outputEntry.instanceID = 0;
            }
#else
            outputEntry.instanceID = GetFieldOrProperty<int>(type, internalEntry, "instanceID");
#endif

            outputEntry.globalLineIndex = GetFieldOrProperty<int>(type, internalEntry, "globalLineIndex");
            outputEntry.callstackTextStartUTF8 = GetFieldOrProperty<int>(type, internalEntry, "callstackTextStartUTF8");
            outputEntry.callstackTextStartUTF16 = GetFieldOrProperty<int>(type, internalEntry, "callstackTextStartUTF16");
        }

        private static void CopyToInternal(LogEntry entry, object internalEntry)
        {
            if (entry == null || internalEntry == null)
                return;

            Type type = internalEntry.GetType();
            SetFieldOrProperty(type, internalEntry, "message", entry.message);
            SetFieldOrProperty(type, internalEntry, "file", entry.file);
            SetFieldOrProperty(type, internalEntry, "mode", entry.mode);
            SetFieldOrProperty(type, internalEntry, "instanceID", entry.instanceID);
#if UNITY_6000_5_OR_NEWER
            SetFieldOrProperty(type, internalEntry, "entityId", entry.entityId);
#endif
        }

        private static T GetFieldOrProperty<T>(Type type, object instance, string name)
        {
            FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null && field.FieldType == typeof(T))
                return (T)field.GetValue(instance);

            PropertyInfo prop = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null && prop.PropertyType == typeof(T))
                return (T)prop.GetValue(instance);

            return default;
        }

        private static void SetFieldOrProperty(Type type, object instance, string name, object value)
        {
            FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                if (value == null)
                {
                    if (!field.FieldType.IsValueType || Nullable.GetUnderlyingType(field.FieldType) != null)
                    {
                        field.SetValue(instance, null);
                        return;
                    }
                }
                else if (field.FieldType.IsAssignableFrom(value.GetType()))
                {
                    field.SetValue(instance, value);
                    return;
                }
            }

            PropertyInfo prop = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null && prop.CanWrite)
                prop.SetValue(instance, value);
        }

        private static MethodInfo FindAddMessagesImpl()
        {
            if (s_Type == null)
                return null;

            MethodInfo[] methods = s_Type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (MethodInfo method in methods)
            {
                if (!string.Equals(method.Name, "AddMessagesImpl", StringComparison.Ordinal))
                    continue;
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == 2 && parameters[1].ParameterType == typeof(int))
                    return method;
            }

            return null;
        }
    }

}
