using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using System;
using Neat.Tools;
using System.Reflection;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    // Editor Functions
    public static partial class EditorFunctions
    {
        public static bool DrawFromSerializedObject(MemberInfo member, SerializedObject obj)
        {
            // try to find serialized property on given object
            // return successful

            var so = obj.FindProperty(member.Name);
            if (so != null)
            {
                foreach (SerializedProperty prop in so)
                    EditorGUILayout.PropertyField(prop);

                return true;
            }
            else
            {
                return false;
            }
        }
        public static void Draw(this SerializedObject obj)
        {
            var iterator = obj.GetIterator();
            iterator.NextVisible(true);
            while (iterator.NextVisible(false))
            {
                iterator.Draw();
            }
        }

        public static void ViewTargetMembers(Object target, MemberInfo[] members)
        {
            foreach (MemberInfo member in members)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel($"{member.Name}");
                //EditorGUILayout.PrefixLabel($"{member.GetValueType()} {member.Name}");
                EditorGUILayout.LabelField($"{member.GetValue(target)}", GUILayout.Width(60));

                try
                {
                    EditorGUILayout.LabelField(member.GetValueType().ToString());
                }
                catch
                {
                    EditorGUILayout.LabelField("?");
                }

                EditorGUILayout.EndHorizontal();
            }
        }


        public static void Draw(this SerializedProperty prop)
        {
            EditorGUILayout.PropertyField(prop);
        }

        public static void IDK()
        {

            // Editor e = Editor.CreateEditor(this);
            // UnityEditor.CustomEditor.IsDefined()
        }
        public static Editor GetCustomEditor(this Object obj)
        {
            bool hasCustomEditor = ActiveEditorTracker.HasCustomEditor(obj);
            Editor e = null;

            if (!hasCustomEditor)
            {
                e = Editor.CreateEditor(obj);

            }
            else
            {

                var editors = ActiveEditorTracker.sharedTracker.activeEditors;

                foreach (var editor in editors)
                {
                    editor.target.GetType();
                }
            }

            return e;
        }

        public static MonoScript GetEditorScript(this MonoBehaviour component)
        {
            return MonoScript.FromMonoBehaviour(component);
        }

        // [InitializeOnLoadMethod]
        public static void PrintEditors()
        {
            var editors = ActiveEditorTracker.sharedTracker.activeEditors;
            Debug.Log($"Found {editors.Length} editors...");
            foreach (var editor in editors)
            {
                Debug.Log($"Editor: {editor.target.GetType()} > {editor.GetType()}");
            }
        }
        public static void EditorScriptField(Editor e)
        {
            var script = MonoScript.FromScriptableObject(e);
            bool wasEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Editor Script");
            EditorGUILayout.ObjectField(script, typeof(MonoScript), true);
            EditorGUILayout.EndHorizontal();
            GUI.enabled = wasEnabled;
        }
        public static void GUIScriptField(GUIInspector inspector)
        {
            var type = inspector.GetType();
            var script = MonoScript.FromScriptableObject(inspector);
            bool wasEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("GUI Script");
            EditorGUILayout.ObjectField(script, typeof(MonoScript), true);
            EditorGUILayout.EndHorizontal();
            GUI.enabled = wasEnabled;
        }

        public static void GUIColorLerp(string label, ref Color a, ref Color b)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            a = EditorGUILayout.ColorField(a);
            b = EditorGUILayout.ColorField(b);
            GUILayout.EndHorizontal();
        }
        public static void GUIMinMaxSlider(string label, ref float a, ref float b, float min, float max)
        {
            GUILayout.BeginHorizontal();
            //GUI.color = Color.red;
            EditorGUILayout.PrefixLabel(label);
            //GUI.color = Color.white;
            a = EditorGUILayout.FloatField(a, GUILayout.Width(40));
            EditorGUILayout.MinMaxSlider(ref a, ref b, min, max);
            b = EditorGUILayout.FloatField(b, GUILayout.Width(40));
            GUILayout.EndHorizontal();
        }
        public static void GUIMinMaxIntSlider(string label, ref int a, ref int b,
            int min, int max)
        {
            float _a = (float)a;
            float _b = (float)b;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            _a = EditorGUILayout.IntField((int)_a, GUILayout.Width(40));
            EditorGUILayout.MinMaxSlider(ref _a, ref _b, min, max);
            _b = EditorGUILayout.IntField((int)_b, GUILayout.Width(40));

            a = (int)_a;
            b = (int)_b;

            GUILayout.EndHorizontal();
        }

        public static void RepaintScene()
        {
            EditorWindow.GetWindow(System.Type.GetType("UnityEditor.GameView,UnityEditor")).Repaint();
        }
        public static EditorWindow GameView()
        {
            // UnityEditor.GameView v;
            var view = EditorWindow.GetWindow(System.Type.GetType("UnityEditor.GameView,UnityEditor"));
            return view;
        }
        public static void GUIListItem<T>(string label, ref List<T> list,
            Action<int> itemClick = null, string clickText = "Select")
        {
            if (list == null)
                list = new List<T>();

            if (!UserData.HasStaticData(list))
                UserData.SetStaticData(list, false); // foldout false

            var stored = UserData.GetStaticData<bool>(list);
            var foldout = EditorGUILayout.Foldout(stored, label);
            UserData.SetStaticData(list, foldout);

            if (!foldout)
                return;

            if (list.Count == 0 && GUILayout.Button("+"))
                list.Add(default);

            for (int i = 0; i < list.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    // remove entry
                    list.RemoveAt(i);
                    i--;
                    continue;
                }
                if (i < list.Count - 1 && GUILayout.Button("↓", GUILayout.Width(30)))
                {
                    // swap with next
                    T temp = list[i];
                    list[i] = list[i + 1];
                    list[i + 1] = temp;
                    i--;
                    continue;
                }

                // show enum popup
                if (typeof(T).IsEnum)
                {
                    // ?????????
                    Enum test = Enum.Parse(typeof(T), list[i].ToString()) as Enum;
                    list[i] = (T)Enum.Parse(typeof(T), EditorGUILayout.EnumPopup(test).ToString());
                }

                // show reference field
                else if (typeof(T).IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    UnityEngine.Object o = list[i] as UnityEngine.Object;
                    var t = (T)Convert.ChangeType(EditorGUILayout.ObjectField(o, typeof(T), true), typeof(T));
                    list[i] = t;
                }

                // show color field
                else if (typeof(T) == typeof(Color))
                {
                    Color c = (Color)Convert.ChangeType(list[i], typeof(Color));
                    list[i] = (T)Convert.ChangeType(EditorGUILayout.ColorField(c), typeof(T));
                }


                if (i != 0 && GUILayout.Button("↑", GUILayout.Width(30)))
                {
                    // swap with previous
                    T temp = list[i];
                    list[i] = list[i - 1];
                    list[i - 1] = temp;
                    i--;
                    continue;
                }
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    // add entry
                    list.Insert(i + 1, default(T));
                    i--;
                    continue;
                }
                GUILayout.EndHorizontal();

                if (itemClick != null)
                {
                    if (GUILayout.Button(clickText))
                        itemClick.Invoke(i);
                }
            }
        }

        public static void GUIListObjectAdd<T>(ref List<T> list, int i,
            Action<int> beforeRemove = null, Action<int> onAdd = null)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                // remove entry
                beforeRemove?.Invoke(i);
                list.RemoveAt(i);
                return;
            }
            if (i < list.Count - 1 && GUILayout.Button("↓", GUILayout.Width(30)))
            {
                // swap with next
                T temp = list[i];
                list[i] = list[i + 1];
                list[i + 1] = temp;
                return;
            }

            // show enum popup
            if (typeof(T).IsEnum)
            {
                // ?????????
                Enum test = Enum.Parse(typeof(T), list[i].ToString()) as Enum;
                list[i] = (T)Enum.Parse(typeof(T), EditorGUILayout.EnumPopup(test).ToString());
            }

            // show reference field
            else if (typeof(T).IsSubclassOf(typeof(UnityEngine.Object)))
            {
                UnityEngine.Object o = list[i] as UnityEngine.Object;
                var t = (T)Convert.ChangeType(EditorGUILayout.ObjectField(o, typeof(T), true), typeof(T));
                list[i] = t;
            }

            // show color field
            else if (typeof(T) == typeof(Color))
            {
                Color c = (Color)Convert.ChangeType(list[i], typeof(Color));
                list[i] = (T)Convert.ChangeType(EditorGUILayout.ColorField(c), typeof(T));
            }


            if (i != 0 && GUILayout.Button("↑", GUILayout.Width(30)))
            {
                // swap with previous
                T temp = list[i];
                list[i] = list[i - 1];
                list[i - 1] = temp;
                return;
            }
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                // add entry
                list.Insert(i + 1, default(T));
                onAdd?.Invoke(i + 1);
                return;
            }
            GUILayout.EndHorizontal();
        }

        public static void GUIListItemWrap<T>(ref List<T> list, Action gui, int i,
            Action<int> onRemove = null, Action<int> onAdd = null,
            Action<int> onMoveUp = null, Action<int> onMoveDown = null) where T : class
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                // remove entry
                onRemove?.Invoke(i);
                list.RemoveAt(i);
                return;
            }
            if (i < list.Count - 1 && GUILayout.Button("↓", GUILayout.Width(30)))
            {
                // swap with next
                T temp = list[i];
                list[i] = list[i + 1];
                list[i + 1] = temp;
                onMoveDown?.Invoke(i);
                return;
            }

            gui.Invoke();

            if (i != 0 && GUILayout.Button("↑", GUILayout.Width(30)))
            {
                // swap with previous
                T temp = list[i];
                list[i] = list[i - 1];
                list[i - 1] = temp;
                onMoveUp?.Invoke(i);
                return;
            }
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                // add entry
                list.Insert(i + 1, default(T));
                onAdd?.Invoke(i + 1);
                return;
            }
            GUILayout.EndHorizontal();
        }

        public static void GUIListItemWrap<T>(Type t, List<T> list, Action gui, int i,
            Action<int> onRemove = null, Action<int> onAdd = null,
            Action<int> onMoveUp = null, Action<int> onMoveDown = null)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                // remove entry
                onRemove?.Invoke(i);
                list.RemoveAt(i);
                return;
            }
            if (i < list.Count - 1 && GUILayout.Button("↓", GUILayout.Width(30)))
            {
                // swap with next
                T temp = list[i];
                list[i] = list[i + 1];
                list[i + 1] = temp;
                onMoveDown?.Invoke(i);
                return;
            }

            gui.Invoke();

            if (i != 0 && GUILayout.Button("↑", GUILayout.Width(30)))
            {
                // swap with previous
                T temp = list[i];
                list[i] = list[i - 1];
                list[i - 1] = temp;
                onMoveUp?.Invoke(i);
                return;
            }
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                // add entry
                list.Insert(i + 1, default(T));
                onAdd?.Invoke(i + 1);
                return;
            }
            GUILayout.EndHorizontal();
        }

        public static void EndLine()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        public static T EnumPopup<T>(string label, T selected) where T : Enum
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            Enum test = Enum.Parse(typeof(T), selected.ToString()) as Enum;
            var newT = (T)Enum.Parse(typeof(T), EditorGUILayout.EnumPopup(test).ToString());
            EditorGUILayout.EndHorizontal();
            return newT;
        }

        public static int IntFieldBounded(string label, int value, int min, int max)
        {
            int x = EditorGUILayout.IntField(label, value);

            if (x < min)
                x = min;
            else if (x > max)
                x = max;
            return x;
        }
        public static float FloatFieldBounded(string label, float value, float min, float max)
        {
            float x = EditorGUILayout.FloatField(label, value);

            if (x < min)
                x = min;
            else if (x > max)
                x = max;
            return x;
        }
    }

}