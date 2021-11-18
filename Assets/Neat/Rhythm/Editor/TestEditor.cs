using UnityEditor;
using UnityEngine;

namespace Neat.Music
{
    [CustomEditor(typeof(TestObj))]
    public class TestObjEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Hello!"))
            {
                Debug.Log("Sup");
            }
        }
    }

    [CustomEditor(typeof(TestScript))]
    public class TestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TestScript _target = (TestScript)target;

            if (GUILayout.Button("Find 'em"))
            {
                _target.InstantiateEm();

                foreach (var o in GetAllInstances<TestObj>())
                {
                    Debug.Log(o.name);
                }
            }
        }

        // https://answers.unity.com/questions/1425758/how-can-i-find-all-instances-of-a-scriptable-objec.html
        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }
    }
}
