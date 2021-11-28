using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Neat.Demos.Fighter
{

    [CustomEditor(typeof(CharacterSelectorComponent))]
    public class CharacterSelectorComponentEditor : Editor
    {
        private CharacterSelectorComponent _target => (CharacterSelectorComponent)target;

        private int selectedIndex = 0;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_target.db == null)
            {
                GUILayout.Label("Character database must be referenced!");
            }
            else
            {
                if (_target.db.GetCharacterNames(out string[] names))
                {
                    int index = EditorGUILayout.Popup("Select", selectedIndex, names);
                    _target.character = _target.db.characters[index];
                }
                else
                {
                    GUILayout.Label("No characters in database. " +
                        "\nCreate one with the character editor!");
                }
            }
        }
    }
}
