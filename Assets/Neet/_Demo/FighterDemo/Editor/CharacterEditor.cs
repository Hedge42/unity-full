using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Neet.Functions;
using System;

namespace Neet.Fighter
{
    [CustomEditor(typeof(CharacterEditorComponent))]
    public class CharacterEditor : Editor
    {
        // needed to pass foldout and index parameters as references
        private class ListData
        {
            public bool foldout = false;
            public int index = -1;
        }

        private ListData characterData;
        private ListData moveData;
        private ListData hitboxData;
        private ListData positionData;
        private ListData stateData;
        private ListData hitboxPositionData;
        private ListData hitboxSizeData;
        private ListData characterTemplateData;
        private ListData moveTemplateData;

        private CharacterDatabase db;
        private CharacterDatabase characterTemplates;
        private MoveDatabase moveTemplates;

        private CharacterEditorComponent _target;

        private int previewFrame = -1;
        private int characterTemplateIndex = 0;
        private int moveTemplateIndex = 0;

        private void OnEnable()
        {
            // character specific
            characterData = new ListData();
            moveData = new ListData();

            // move specific
            positionData = new ListData();
            stateData = new ListData();
            hitboxData = new ListData();

            // hitbox specifc
            hitboxPositionData = new ListData();
            hitboxSizeData = new ListData();

            // templates
            characterTemplateData = new ListData();
            characterTemplateData.index = 0;

            moveTemplateData = new ListData();
            moveTemplateData.index = 0;
        }
        private void OnDisable()
        {
            _target = (CharacterEditorComponent)target;
            _target.active = false;
        }

        private void OnSceneGUI()
        {
            _target = (CharacterEditorComponent)target;

            UpdatePreview(Selection.activeGameObject == _target.gameObject);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _target = (CharacterEditorComponent)target;

            // EditorGUI.BeginChangeCheck();

            db = _target.db;
            moveTemplates = _target.moveTemplates;
            characterTemplates = _target.characterTemplates;

            if (db != null)
            {
                MakeList(typeof(Character), db.characters, characterData,
                    "Characters", ResetCharacterListData, GetCharacterName);
                EditCharacter();
            }

            //if (EditorGUI.EndChangeCheck())
            //{
            //}
        }

        private void ResetAllListData()
        {
            characterData = new ListData();

            ResetCharacterListData();
        }
        private void ResetCharacterListData()
        {
            moveData = new ListData();

            ResetMoveListData();
        }
        private void ResetMoveListData()
        {
            hitboxData = new ListData();
            stateData = new ListData();
            positionData = new ListData();

            ResetHitboxListData();
        }
        private void ResetHitboxListData()
        {
            hitboxPositionData = new ListData();
            hitboxSizeData = new ListData();
        }

        private bool GetCurrent<T>(out T t, ListData data, List<T> list)
        {
            t = default(T);

            if (data.index >= 0)
            {
                // potential error case
                if (data.index >= list.Count)
                {
                    Debug.LogError(typeof(T).ToString() + " index out of range");
                    return false;
                }
                // sole success case
                else
                {
                    t = list[data.index];
                    return true;
                }
            }
            else
                return false;
        }

        private string GetMoveName(int i)
        {
            try
            {
                return db.characters[characterData.index].moves[i].name;
            }
            catch
            {
                return "null";
            }
        }
        private string GetCharacterName(int i)
        {
            try
            {
                return db.characters[i].name;
            }
            catch
            {
                return "null";
            }
        }

        private void MakeList<T>(Type t, List<T> list, ListData data,
            string label, Action onEditClick = null,
            Func<int, string> getName = null) where T : new()
        {
            data.foldout = EditorGUILayout.Foldout(data.foldout,
                label + " (" + list.Count + ")");

            if (data.foldout)
            {
                if (list.Count == 0 && GUILayout.Button("+"))
                    list.Add(new T());

                for (int i = 0; i < list.Count; i++)
                {
                    string text = "";
                    if (getName != null)
                        text = getName(i);
                    else
                        text = label + "[" + i + "]";


                    Action gui = delegate
                    {
                        if (data.index == i)
                        {
                            // https://www.reddit.com/r/Unity3D/comments/88zt5f/how_do_i_change_the_color_of_a_guilayout_button/
                            Color bg = GUI.backgroundColor;
                            GUI.backgroundColor = Color.green;
                            if (GUILayout.Button("Stop editing " + text))
                                data.index = -1;
                            GUI.backgroundColor = bg;
                        }
                        else
                        {
                            if (GUILayout.Button("Edit " + text))
                            {
                                onEditClick?.Invoke();
                                data.index = i;
                            }
                        }
                    };

                    Action<int> onRemove = delegate (int x)
                    {
                        ItemRemoved(x, data);
                    };
                    Action<int> onAdd = delegate (int x)
                    {
                        ItemAdded(list, x);
                    };
                    Action<int> onMoveUp = delegate (int x)
                    {
                        ItemMovedUp(x, data);
                    };
                    Action<int> onMoveDown = delegate (int x)
                    {
                        ItemMovedDown(x, data);
                    };

                    EditorFunctions.GUIListItemWrap(t, list, gui, i,
                        onRemove: onRemove, onAdd: onAdd,
                        onMoveUp: onMoveUp, onMoveDown: onMoveDown);
                }
            }
            else
            {
                data.index = -1;
            }
        }

        private void UpdatePreview(bool selected)
        {
            try
            {
                db = _target.db;
                _target.active = selected;
                Character c = null;
                Move m = null;

                if (GetCurrent(out c, characterData, db.characters))
                    GetCurrent(out m, moveData, c.moves);

                _target.SetState(c, m, previewFrame);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                _target.active = false;
            }

            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        private void ItemAdded<T>(List<T> list, int newIndex) where T : new()
        {
            list[newIndex] = new T();
        }
        private void ItemRemoved(int removedIndex, ListData data)
        {
            if (removedIndex == data.index)
                data.index = -1;
        }
        private void ItemMovedUp(int movingIndex, ListData data)
        {
            if (movingIndex == data.index)
                data.index -= 1;
            else if (movingIndex == data.index - 1)
                data.index += 1;
        }
        private void ItemMovedDown(int movingIndex, ListData data)
        {
            if (movingIndex == data.index)
                data.index += 1;
            else if (movingIndex == data.index + 1)
                data.index -= 1;
        }

        private void EditCharacter()
        {
            if (GetCurrent(out Character c, characterData, db.characters))
            {
                HandleCharacterTemplate();

                c.name = EditorGUILayout.TextField("Name", c.name);
                c.size = EditorGUILayout.Vector2Field("Size", c.size);
                c.walkSpeed = EditorGUILayout.FloatField("Walkspeed", c.walkSpeed);
                c.health = EditorGUILayout.IntField("Health", c.health);

                EditorFunctions.EndLine();

                MakeList(typeof(Move), c.moves, moveData, "Moves", ResetMoveListData, GetMoveName);
                EditMove(c);
            }
        }
        private void EditMove(Character c)
        {
            if (GetCurrent(out Move m, moveData, c.moves))
            {
                EditorGUILayout.LabelField(m.name + " properties",
                    EditorStyles.boldLabel);

                HandleMoveTemplate();

                m.name = EditorGUILayout.TextField("Name", m.name);
                m.frames = EditorFunctions.IntFieldBounded("Frames", m.frames, 0, 300);
                m.state = EditorGUILayout.TextField("Input State", m.state);
                m.directions = EditorGUILayout.TextField("Directions", m.directions);
                m.buttons = EditorGUILayout.TextField("Buttons", m.buttons);

                previewFrame = EditorGUILayout.IntSlider("Preview frame", previewFrame, 0,
                c.moves[moveData.index].frames);

                m.ValidateFrameBounds();

                EditorFunctions.EndLine();

                MakeList(typeof(Hitbox), m.hitboxes, hitboxData, "Hitboxes", ResetHitboxListData);
                EditHitboxs(m);

                MakeList(typeof(StateChange), m.stateChanges, stateData, "States");
                EditStates(m);

                MakeList(typeof(VectorInterpolation), m.positionChanges, positionData, "Positions");
                EditPositions(m);
            }
        }
        private void EditHitboxs(Move m)
        {
            if (GetCurrent(out Hitbox h, hitboxData, m.hitboxes))
            {
                EditorFunctions.GUIMinMaxIntSlider("Frames", ref h.startFrame,
                    ref h.endFrame, 0, m.frames);

                h.damage = EditorFunctions.IntFieldBounded("Damage", h.damage, -50, 1000);
                h.level = EditorFunctions.EnumPopup("Attack level", h.level);
                h.startPos = EditorGUILayout.Vector2Field("Start pos", h.startPos);
                h.startSize = EditorGUILayout.Vector2Field("Start size", h.startSize);

                h.disconnect = EditorGUILayout.Toggle("Disconnect", h.disconnect);

                if (h.disconnect)
                {
                    h.disconnectFrame = EditorFunctions.IntFieldBounded(
                        "Disconnect frame", h.disconnectFrame, h.startFrame, h.endFrame);
                }

                MakeList(typeof(VectorInterpolation), h.ipPosition, hitboxPositionData,
                    "Hitbox position adjustments");
                EditHitboxPositions(h);

                MakeList(typeof(VectorInterpolation), h.ipSize, hitboxSizeData,
                    "Hitbox size adjustments");
                EditHitboxSizeAdjustments(h);

                EditorFunctions.EndLine();
            }
        }
        private void EditStates(Move m)
        {
            if (GetCurrent(out StateChange s, stateData, m.stateChanges))
            {
                s.frame = EditorFunctions.IntFieldBounded("Frame", s.frame, 0, m.frames - 1);

                s.ground = EditorGUILayout.Toggle("Ground", s.ground);
                s.stand = EditorGUILayout.Toggle("Stand", s.stand);
                s.attack = EditorGUILayout.Toggle("Attack", s.attack);
                s.guard = EditorGUILayout.Toggle("Guard", s.guard);
                s.knockdown = EditorGUILayout.Toggle("Knockdown", s.knockdown);

                EditorFunctions.EndLine();
            }
        }
        private void EditPositions(Move m)
        {
            if (GetCurrent(out VectorInterpolation v, positionData, m.positionChanges))
            {
                EditorFunctions.GUIMinMaxIntSlider("Frames", ref v.startFrame,
                    ref v.endFrame, 0, m.frames);

                v.power = EditorFunctions.FloatFieldBounded("Power", v.power, 0, 10);
                v.change = EditorGUILayout.Vector2Field("Delta", v.change);

                EditorFunctions.EndLine();
            }
        }
        private void EditHitboxPositions(Hitbox h)
        {
            if (GetCurrent(out VectorInterpolation v, hitboxPositionData, h.ipPosition))
            {
                EditorFunctions.GUIMinMaxIntSlider("Frames", ref v.startFrame,
                    ref v.endFrame, h.startFrame, h.endFrame);

                v.power = EditorFunctions.FloatFieldBounded("Power", v.power, 0, 10);
                v.change = EditorGUILayout.Vector2Field("Delta", v.change);
            }
        }
        private void EditHitboxSizeAdjustments(Hitbox h)
        {
            if (GetCurrent(out VectorInterpolation v, hitboxSizeData, h.ipSize))
            {
                EditorFunctions.GUIMinMaxIntSlider("Frames", ref v.startFrame,
                    ref v.endFrame, h.startFrame, h.endFrame);

                v.power = EditorFunctions.FloatFieldBounded("Power", v.power, 0, 10);
                v.change = EditorGUILayout.Vector2Field("Delta", v.change);
            }
        }

        private void HandleCharacterTemplate()
        {
            if (characterTemplates != null)
            {
                characterTemplateData.foldout = EditorGUILayout.
                    Foldout(characterTemplateData.foldout, "Character templates");

                bool hasTemplates = characterTemplates.GetCharacterNames(out string[] names);

                if (characterTemplateData.foldout)
                {
                    if (hasTemplates)
                        characterTemplateIndex = EditorGUILayout.Popup("Select", characterTemplateIndex, names);

                    if (GUILayout.Button("Add this character to templates"))
                    {
                        Character clone = db.characters[characterData.index].Clone();
                        characterTemplates.characters.Add(clone);
                    }

                    if (hasTemplates)
                    {
                        if (GUILayout.Button("Remove selected template"))
                        {
                            characterTemplates.characters.RemoveAt(characterTemplateIndex);
                        }

                        if (GUILayout.Button("Overwrite template with this character"))
                        {
                            Character clone = db.characters[characterData.index].Clone();
                            characterTemplates.characters[characterTemplateIndex] = clone;
                        }

                        if (GUILayout.Button("Overwrite character from this template"))
                        {
                            // clone the character
                            Character clone = characterTemplates.characters[characterTemplateIndex].Clone();
                            db.characters[characterData.index] = clone;
                        }

                    }
                    EditorFunctions.EndLine();
                }
            }
        }
        private void HandleMoveTemplate()
        {
            if (moveTemplates != null)
            {
                moveTemplateData.foldout = EditorGUILayout.Foldout(moveTemplateData.foldout, "Move templates");

                bool hasTemplates = moveTemplates.GetMoveNames(out string[] names);

                if (moveTemplateData.foldout)
                {
                    if (hasTemplates)
                        moveTemplateIndex = EditorGUILayout.Popup("Select move template", moveTemplateIndex, names);

                    if (GUILayout.Button("Add this move to templates"))
                    {
                        Move clone = db.characters[characterData.index].moves[moveData.index].Clone();
                        moveTemplates.moves.Add(clone);
                    }

                    if (hasTemplates)
                    {

                        if (GUILayout.Button("Remove selected template"))
                        {
                            moveTemplates.moves.RemoveAt(moveTemplateIndex);
                        }

                        if (GUILayout.Button("Overwrite template with this move"))
                        {
                            Move clone = db.characters[characterData.index].moves[moveData.index].Clone();
                            moveTemplates.moves[moveTemplateIndex] = clone;
                        }

                        if (GUILayout.Button("Overwrite move from this template"))
                        {
                            // clone the character
                            Move clone = moveTemplates.moves[moveTemplateIndex].Clone();
                            db.characters[characterData.index].moves[moveData.index] = clone;
                        }
                    }

                    EditorFunctions.EndLine();
                }
            }
        }
    }
}
