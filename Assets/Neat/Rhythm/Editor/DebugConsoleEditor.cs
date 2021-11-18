using UnityEditor;
using UnityEngine;

namespace Neat.Console
{
    using UnityEditor;
    using UnityEditor.UIElements;

    public class ExtendedEditorWindow : EditorWindow
    {
        // https://www.youtube.com/watch?v=c_3DXBrH-Is
        protected SerializedObject serializedObject;
        protected SerializedProperty currentProperty;

        private string selectedPropertyPath;
        protected SerializedProperty selectedProperty;

        protected void DrawProperties(SerializedProperty prop, bool drawChildren)
        {
            string lastPropPath = string.Empty;
            foreach (SerializedProperty p in prop)
            {
                if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUILayout.BeginHorizontal();
                    p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                    EditorGUILayout.EndHorizontal();

                    if (p.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        DrawProperties(p, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath))
                        continue;

                    lastPropPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        }
        protected void DrawSidebar(SerializedProperty prop)
        {
            foreach (SerializedProperty p in prop)
            {
                if (GUILayout.Button(p.displayName))
                {
                    selectedPropertyPath = p.propertyPath;
                }
            }

            if (!string.IsNullOrEmpty(selectedPropertyPath))
            {
                selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
            }
        }
        protected void DrawField(string propName, bool relative)
        {
            if (relative && currentProperty != null)
            {
                EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative(propName), true);
            }
            else if (serializedObject != null)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(propName), true);
            }
        }
        protected void DrawSelectedPropertiesPanel()
        {
            currentProperty = selectedProperty;

            EditorGUILayout.BeginHorizontal("box");
            // DrawField("name", true);
            DrawField("title", true);
            EditorGUILayout.EndHorizontal();
        }

        protected void Apply()
        {
            serializedObject.ApplyModifiedProperties();
        }

    }
    public class DebugConsoleEditorWindow : ExtendedEditorWindow
    {
        //[MenuItem("Cheat Console Editor")]
        //public static void Open(DebugConsole console)
        //{
        //    var window = GetWindow<DebugConsoleEditorWindow>("Cheat Console Editor");
        //    window.serializedObject = new SerializedObject(console);
        //}
        //private void OnGUI()
        //{
        //    // serializedObject.get
        //    // currentProperty = serializedObject.FindProperty("_commands");
        //    // DrawProperties(currentProperty, true);

        //    //var x = serializedObject.GetIterator();
        //    //x.Next(true);
        //    //while (x.NextVisible(false))
        //    //    DrawProperties(x, true);
        //    // DrawField("commands", true);
        //    //DrawField("show", true);
        //    //DrawField("submit", true);
        //    //DrawField("cancel", true);

        //    // DrawSidebar(serializedObject.FindProperty("tests"));

        //    // DrawField("commands", true);

        //    //var it = serializedObject.GetIterator();
        //    //it.Next(true);

        //    //while (it.NextVisible(false))
        //    //{
        //    //DrawProperties(it, false);
        //    //}


        //    // currentProperty = serializedObject.FindProperty("_commands");
        //    // DrawProperties(currentProperty, true);

        //    //var x = serializedObject.GetIterator();

        //    //while (x.NextVisible(false))
        //    //{

        //    //}




        //    //EditorGUILayout.BeginHorizontal();
        //    //EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        //    //DrawSidebar(currentProperty);
        //    //EditorGUILayout.EndVertical();

        //    //EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        //    //if (selectedProperty != null)
        //    //{
        //    //    DrawSelectedPropertiesPanel();
        //    //}
        //    //else
        //    //{
        //    //    EditorGUILayout.LabelField("Select something...");
        //    //}
        //    //EditorGUILayout.EndVertical();
        //    //EditorGUILayout.EndHorizontal();

        //    // Apply();
        //}
    }


    [CustomEditor(typeof(DebugConsole))]
    public class DebugConsoleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DebugConsole _target = target as DebugConsole;

            if (GUILayout.Button("Open Window"))
            {
                Debug.Log("lol");
            }

            //foreach (var cmd in _target.commands)
            //{
                //GUILayout.Label(cmd.ToString());
            //}


            // https://youtu.be/mTjYA3gC1hA?t=255
            //var serializedProperty = serializedObject.GetIterator();
            //while (serializedProperty.NextVisible(false))
            //{
            //    PropertyField prop = new PropertyField(serializedProperty);
            //    // prop.SetEnabled(prop.name != "help");
            //}
        }
    }
}