using UnityEditor;
using UnityEngine;

namespace Neat.Tools
{
    public class InspectorWindow : EditorWindow
    {
        protected Editor inspector;
        [SerializeField] protected Object obj;

        public static void Open<T>(T obj) where T : Object
        {
            var _type = obj.GetType();
            InspectorWindow window = GetWindow<InspectorWindow>($"{_type.Name} Inspector Window");
            window.Create(obj);
        }

        protected void Create(Object obj)
        {
            this.obj = obj;
            this.inspector = Editor.CreateEditor(obj);
        }

        private void OnEnable()
        {
            // inspector = Editor.CreateEditor(obj);
            Debug.Log("Enabling...");
            //this.inspector = Editor.CreateEditor(inspector.target);
            this.inspector = Editor.CreateEditor(obj);
        }
        private void OnGUI()
        {
            try
            {
                inspector.OnInspectorGUI();
            }
            catch
            {
                inspector = Editor.CreateEditor(inspector.target);
                Debug.Log($"null: ins_s_obj({inspector.serializedObject == null}), inspector_target({inspector.target == null})");
            }
        }
    }
}
