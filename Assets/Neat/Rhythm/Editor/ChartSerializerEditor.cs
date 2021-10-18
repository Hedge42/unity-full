using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Neat.Audio;

namespace Neat.Music
{
    [ExecuteInEditMode] // for onEnable ???
    [CustomEditor(typeof(ChartSerializer))]
    public class ChartSerializerEditor : Editor
    {
        private ChartSerializer _target => (ChartSerializer)target;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            HandleChartSelect();
            HandleText();


            // buttons
            if (GUILayout.Button("New Chart"))
            {
                _target.Load(new Chart());
            }
            else if (GUILayout.Button("Save"))
            {
                _target.Save();
                ReadPaths();
            }
            else if (GUILayout.Button("Open chart directory"))
            {
                ChartSerializer.OpenDirectory(); // why am I calling Chart?
            }
            else if (GUILayout.Button("Print chart info"))
            {
                Debug.Log(_target.chart.ToString(), _target);
            }


            HandleNoteMapSelect();
            if (GUILayout.Button("New NoteMap"))
            {
                NewNoteMapClick();
            }
            else if (GUILayout.Button("Delete NoteMap"))
            {
                DeleteNoteMapClick();
            }
            //HandlePathInputLoad();
        }
        private void OnEnable()
        {
            SwitchToLoaded();
        }

        private void HandleText()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("File path");
            EditorGUI.BeginDisabledGroup(true);
            GUILayout.TextField(_target.chart.filePath);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Music path");
            _target.chart.musicPath = GUILayout.TextField(_target.chart.musicPath);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Name");
            _target.chart.name = GUILayout.TextField(_target.chart.name);
            EditorGUILayout.EndHorizontal();
        }

        // chart select
        private int selectedChart = 0;
        private string[] chartPaths;
        private string[] chartNames;
        private void HandleChartSelect()
        {
            // what about when a new one is created?
            if (chartPaths == null)
            {
                ReadPaths();
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Select Chart");

            EditorGUI.BeginChangeCheck();
            selectedChart = EditorGUILayout.Popup(selectedChart, chartNames);
            if (EditorGUI.EndChangeCheck())
            {
                // PlayerPrefs.SetString(GetType().ToString(), chartPaths[selectedChart]);

                _target.Load(chartPaths[selectedChart]);
            }
            EditorGUILayout.EndHorizontal();
        }

        // notemap gui
        private int selectedNoteMap = 0;
        private void HandleNoteMapSelect()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Select Note Map");

            // get string array of notemap names
            var names = new List<string>();
            foreach (var o in _target.chart.noteMaps)
                names.Add(o.ToString());

            EditorGUI.BeginChangeCheck();
            selectedNoteMap = EditorGUILayout.Popup(selectedNoteMap, names.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("Selecting noteMap: " + _target.chart.noteMaps[selectedNoteMap]);
            }

            GUILayout.EndHorizontal();
        }
        private void NewNoteMapClick()
        {
            Debug.Log("Not set up");
            // new NoteMap(new GuitarTuning());
        }
        private void DeleteNoteMapClick()
        {
            Debug.Log("Not set up");
            // new NoteMap(new GuitarTuning());
        }

        private void ReadPaths()
        {
            chartPaths = _target.GetPaths();
            chartNames = _target.GetFileNames(chartPaths);
        }
        private void SwitchToLoaded()
        {
            ReadPaths();
            var fp = _target.chart.filePath;
            for (int i = 0; i < chartPaths.Length; i++)
            {
                if (fp.Equals(chartPaths[i]))
                {
                    selectedChart = i;
                }
            }
        }

        // load from outside of main chart directory
        private string loadPath = "";
        private void HandlePathInputLoad()
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.PrefixLabel("Path: ");
            loadPath = EditorGUILayout.TextField(loadPath);

            if (GUILayout.Button("Load"))
                _target.Load(loadPath);

            GUILayout.EndHorizontal();
        }
    }
}
