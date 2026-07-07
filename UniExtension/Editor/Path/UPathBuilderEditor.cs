#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace UniExtension.Path
{
    [CustomEditor(typeof(UPathBuilder))]
    public sealed class UPathBuilderEditor : UnityEditor.Editor
    {
        private int selectedIndex = -1;

        private SerializedProperty applyBuilderTransformProperty;
        private SerializedProperty offsetProperty;

        private SerializedProperty addPointPlaneProperty;
        private SerializedProperty drawLinesProperty;
        private SerializedProperty drawLabelsProperty;
        private SerializedProperty handleSizeProperty;

        private void OnEnable()
        {
            applyBuilderTransformProperty = serializedObject.FindProperty("applyBuilderTransform");
            offsetProperty = serializedObject.FindProperty("offset");

            addPointPlaneProperty = serializedObject.FindProperty("addPointPlane");
            drawLinesProperty = serializedObject.FindProperty("drawLines");
            drawLabelsProperty = serializedObject.FindProperty("drawLabels");
            handleSizeProperty = serializedObject.FindProperty("handleSize");
        }

        public override void OnInspectorGUI()
        {
            UPathBuilder builder = (UPathBuilder)target;
            builder.EnsurePath();

            serializedObject.Update();

            EditorGUILayout.LabelField("Final Result", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(
                applyBuilderTransformProperty,
                new GUIContent("Apply Builder Transform")
            );

            EditorGUILayout.PropertyField(
                offsetProperty,
                new GUIContent("Offset")
            );

            using (new EditorGUI.DisabledScope(builder.Offset == Vector3.zero))
            {
                if (GUILayout.Button("Apply Offset"))
                {
                    Undo.RecordObject(builder, "Apply Path Offset");

                    builder.ApplyOffsetToPoints();

                    MarkChanged(builder);
                    serializedObject.Update();
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Scene Edit", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(addPointPlaneProperty);
            EditorGUILayout.PropertyField(drawLinesProperty);
            EditorGUILayout.PropertyField(drawLabelsProperty);
            EditorGUILayout.PropertyField(handleSizeProperty);

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            DrawPathInfo(builder);
            DrawToolbar(builder);
            DrawPointList(builder);
        }

        private void OnSceneGUI()
        {
            UPathBuilder builder = (UPathBuilder)target;
            builder.EnsurePath();

            ClampSelectedIndex(builder);

            Event e = Event.current;

            HandleSceneInput(builder, e);

            DrawSceneHelp(builder);
            DrawPathLines(builder);
            DrawPointHandles(builder);
        }

        private void DrawPathInfo(UPathBuilder builder)
        {
            EditorGUILayout.LabelField("Path Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Point Count", builder.Path.Count.ToString());
            EditorGUILayout.LabelField("Raw Length", builder.Path.Length.ToString("0.###"));
            EditorGUILayout.LabelField("Final Length", builder.FinalLength.ToString("0.###"));

            EditorGUILayout.Space();

            EditorGUILayout.HelpBox(
                "Raw Point는 Path에 저장된 원본 값입니다.\n" +
                "Final Point는 Offset과 Apply Builder Transform이 적용된 최종 결과입니다.",
                MessageType.Info
            );
        }

        private void DrawToolbar(UPathBuilder builder)
        {
            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Add Point"))
                {
                    AddPoint(builder, GetDefaultNewRawPoint(builder, builder.Path.Count));
                }

                using (new EditorGUI.DisabledScope(selectedIndex < 0 || selectedIndex >= builder.Path.Count))
                {
                    if (GUILayout.Button("Insert After Selected"))
                    {
                        int insertIndex = selectedIndex + 1;
                        InsertPoint(builder, insertIndex, GetDefaultNewRawPoint(builder, insertIndex));
                    }

                    if (GUILayout.Button("Remove Selected"))
                    {
                        RemovePoint(builder, selectedIndex);
                    }
                }

                using (new EditorGUI.DisabledScope(builder.Path.Count == 0))
                {
                    if (GUILayout.Button("Clear"))
                    {
                        ClearPath(builder);
                    }
                }
            }

            EditorGUILayout.HelpBox(
                "SceneView\n" +
                "Shift + Left Click: Add Point\n" +
                "Ctrl + Shift + Left Click: Insert After Selected\n" +
                "Click Point: Select\n" +
                "Delete / Backspace: Remove Selected",
                MessageType.Info
            );
        }

        private void DrawPointList(UPathBuilder builder)
        {
            EditorGUILayout.Space();

            if (builder.Path.Count == 0)
            {
                EditorGUILayout.HelpBox("No points. Use Add Point or Shift + Left Click in SceneView.", MessageType.None);
                return;
            }

            EditorGUILayout.LabelField("Raw Points", EditorStyles.boldLabel);

            for (int i = 0; i < builder.Path.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    bool selected = selectedIndex == i;

                    bool nextSelected = GUILayout.Toggle(
                        selected,
                        i.ToString(),
                        EditorStyles.miniButton,
                        GUILayout.Width(32)
                    );

                    if (nextSelected && selectedIndex != i)
                    {
                        selectedIndex = i;
                        SceneView.RepaintAll();
                    }

                    EditorGUI.BeginChangeCheck();

                    Vector3 newRawPoint = EditorGUILayout.Vector3Field(GUIContent.none, builder.Path[i]);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(builder, "Edit Path Point");

                        builder.Path[i] = newRawPoint;

                        MarkChanged(builder);
                    }

                    if (GUILayout.Button("+", GUILayout.Width(24)))
                    {
                        InsertPoint(builder, i + 1, GetDefaultNewRawPoint(builder, i + 1));
                        break;
                    }

                    if (GUILayout.Button("-", GUILayout.Width(24)))
                    {
                        RemovePoint(builder, i);
                        break;
                    }
                }

                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.Vector3Field("Final P" + i, builder.GetFinalPoint(i));
                }
            }
        }

        private void HandleSceneInput(UPathBuilder builder, Event e)
        {
            if (e == null)
                return;

            if (e.type == EventType.Layout && e.shift && !e.alt)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }

            if (e.type == EventType.KeyDown)
            {
                if (selectedIndex >= 0 &&
                    selectedIndex < builder.Path.Count &&
                    (e.keyCode == KeyCode.Delete || e.keyCode == KeyCode.Backspace))
                {
                    RemovePoint(builder, selectedIndex);
                    e.Use();
                    return;
                }
            }

            if (e.type != EventType.MouseDown)
                return;

            if (e.button != 0)
                return;

            if (!e.shift || e.alt)
                return;

            Vector3 finalPoint;

            if (!TryGetFinalPointOnEditPlane(builder, e.mousePosition, out finalPoint))
                return;

            Vector3 rawPoint = builder.ToRawPoint(finalPoint);

            bool insertAfterSelected =
                (e.control || e.command) &&
                selectedIndex >= 0 &&
                selectedIndex < builder.Path.Count;

            if (insertAfterSelected)
            {
                InsertPoint(builder, selectedIndex + 1, rawPoint);
            }
            else
            {
                AddPoint(builder, rawPoint);
            }

            e.Use();
        }

        private void DrawSceneHelp(UPathBuilder builder)
        {
            Handles.BeginGUI();

            GUILayout.BeginArea(new Rect(10f, 10f, 390f, 116f), EditorStyles.helpBox);
            GUILayout.Label("UPath Builder", EditorStyles.boldLabel);
            GUILayout.Label("Shift + Left Click: Add Point");
            GUILayout.Label("Ctrl + Shift + Left Click: Insert After Selected");
            GUILayout.Label("Click Point: Select");
            GUILayout.Label("Delete / Backspace: Remove Selected");
            GUILayout.Label("Apply Transform: " + builder.ApplyBuilderTransform);
            GUILayout.Label("Offset: " + builder.Offset);
            GUILayout.EndArea();

            Handles.EndGUI();
        }

        private void DrawPathLines(UPathBuilder builder)
        {
            if (!builder.DrawLines)
                return;

            if (builder.Path.Count < 2)
                return;

            Color oldColor = Handles.color;

            Handles.color = Color.green;
            Handles.DrawAAPolyLine(4f, builder.GetFinalPoints());

            Handles.color = oldColor;
        }

        private void DrawPointHandles(UPathBuilder builder)
        {
            for (int i = 0; i < builder.Path.Count; i++)
            {
                Vector3 finalPoint = builder.GetFinalPoint(i);
                float size = HandleUtility.GetHandleSize(finalPoint) * builder.HandleSize;

                Color oldColor = Handles.color;
                Handles.color = selectedIndex == i ? Color.yellow : Color.cyan;

                if (Handles.Button(
                        finalPoint,
                        Quaternion.identity,
                        size,
                        size * 1.4f,
                        Handles.SphereHandleCap))
                {
                    selectedIndex = i;
                    Repaint();
                    SceneView.RepaintAll();
                }

                if (selectedIndex == i)
                {
                    EditorGUI.BeginChangeCheck();

                    Vector3 newFinalPoint = Handles.PositionHandle(finalPoint, Quaternion.identity);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(builder, "Move Path Point");

                        builder.Path[i] = builder.ToRawPoint(newFinalPoint);

                        MarkChanged(builder);
                    }
                }

                if (builder.DrawLabels)
                {
                    Handles.Label(finalPoint + Vector3.up * size * 2f, "P" + i);
                }

                Handles.color = oldColor;
            }
        }

        private void AddPoint(UPathBuilder builder, Vector3 rawPoint)
        {
            Undo.RecordObject(builder, "Add Path Point");

            builder.Path.AddPoint(rawPoint);
            selectedIndex = builder.Path.Count - 1;

            MarkChanged(builder);
        }

        private void InsertPoint(UPathBuilder builder, int index, Vector3 rawPoint)
        {
            index = Mathf.Clamp(index, 0, builder.Path.Count);

            Undo.RecordObject(builder, "Insert Path Point");

            builder.Path.InsertPoint(index, rawPoint);
            selectedIndex = index;

            MarkChanged(builder);
        }

        private void RemovePoint(UPathBuilder builder, int index)
        {
            if (index < 0 || index >= builder.Path.Count)
                return;

            Undo.RecordObject(builder, "Remove Path Point");

            builder.Path.RemoveAt(index);

            if (builder.Path.Count == 0)
                selectedIndex = -1;
            else
                selectedIndex = Mathf.Clamp(index, 0, builder.Path.Count - 1);

            MarkChanged(builder);
        }

        private void ClearPath(UPathBuilder builder)
        {
            Undo.RecordObject(builder, "Clear Path");

            builder.Path.Clear();
            selectedIndex = -1;

            MarkChanged(builder);
        }

        private void ClampSelectedIndex(UPathBuilder builder)
        {
            if (builder.Path.Count == 0)
            {
                selectedIndex = -1;
                return;
            }

            if (selectedIndex >= builder.Path.Count)
                selectedIndex = builder.Path.Count - 1;
        }

        private static Vector3 GetDefaultNewRawPoint(UPathBuilder builder, int index)
        {
            UPath path = builder.Path;

            if (path.Count == 0)
                return builder.ToRawPoint(builder.transform.position);

            if (index > 0 && index < path.Count)
                return (path[index - 1] + path[index]) * 0.5f;

            if (index <= 0)
                return path[0] - Vector3.forward;

            return path[path.Count - 1] + Vector3.forward;
        }

        private static bool TryGetFinalPointOnEditPlane(
            UPathBuilder builder,
            Vector2 guiPosition,
            out Vector3 finalPoint)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(guiPosition);

            Vector3 normal;

            switch (builder.AddPointPlane)
            {
                case UPathEditPlane.XY:
                    normal = Vector3.forward;
                    break;

                case UPathEditPlane.YZ:
                    normal = Vector3.right;
                    break;

                case UPathEditPlane.View:
                    if (SceneView.currentDrawingSceneView != null &&
                        SceneView.currentDrawingSceneView.camera != null)
                    {
                        normal = SceneView.currentDrawingSceneView.camera.transform.forward;
                    }
                    else
                    {
                        normal = Vector3.forward;
                    }
                    break;

                case UPathEditPlane.XZ:
                default:
                    normal = Vector3.up;
                    break;
            }

            Vector3 planePoint = builder.transform.position;

            if (builder.Path.Count > 0)
            {
                int index = Mathf.Clamp(builder.Path.Count - 1, 0, builder.Path.Count - 1);
                planePoint = builder.GetFinalPoint(index);
            }

            Plane plane = new Plane(normal, planePoint);

            float enter;

            if (plane.Raycast(ray, out enter))
            {
                finalPoint = ray.GetPoint(enter);
                return true;
            }

            finalPoint = Vector3.zero;
            return false;
        }

        private static void MarkChanged(UPathBuilder builder)
        {
            EditorUtility.SetDirty(builder);
            PrefabUtility.RecordPrefabInstancePropertyModifications(builder);
            SceneView.RepaintAll();
        }
    }
}

#endif