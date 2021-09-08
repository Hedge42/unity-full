using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
	[System.Serializable]
	private class VectorTest
	{
		public float x = 6;
		public float y = 9;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var _target = (SaveManager)target;

		if (GUILayout.Button("Open file location"))
		{
			_target.OpenFileLocation();
		}

		if (GUILayout.Button("Save"))
		{
			_target.Save();
		}
		if (GUILayout.Button("Load"))
		{
			_target.Load();
		}

		if (GUILayout.Button("Add a vector"))
		{
			_target.AddData(new VectorTest());
		}

		if (GUILayout.Button("Print data"))
		{
			Debug.Log(_target.GetObjects());
		}


	}
}
