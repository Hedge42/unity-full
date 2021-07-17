using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Fighter
{
    public class InputHandler : MonoBehaviour
    {
        private const int dirBuffer = 60;
        private const int btnBuffer = 5;

        public string dirInput;
        private List<int> btnInput;

        private List<InputEvent> btnStream;
        private List<InputEvent> dirStream;

        public InputSetting input;

        private Command[] cmdList;

        private void Start()
        {
            Time.fixedDeltaTime = 1f / 60f;
            Application.targetFrameRate = 60;

            dirInput = "";
            btnInput = new List<int>();

            dirStream = new List<InputEvent>();
            btnStream = new List<InputEvent>();

            for (int i = 0; i < dirBuffer; i++)
            {
                dirStream.Add(new InputEvent(5));
                btnStream.Add(new InputEvent());

                dirInput += "5";
                btnInput.Add(0);
            }
            CreateList();
        }

        private void FixedUpdate()
        {
            UpdateInputEvents();
            ParseEvents();
        }

        private void HandleInput()
        {
            Vector2 vec = GetDirectionalVector();
            string str = GetDirectionString(vec);
            UpdateDirInput(str);
            UpdateButtonInput();
        }

        // updating input
        private void UpdateInputEvents()
        {
            InputEvent btn = new InputEvent();

            if (UnityEngine.Input.GetKeyDown(input.punch))
                btn.valueDown += 1;
            if (UnityEngine.Input.GetKeyDown(input.kick))
                btn.valueDown += 2;
            if (UnityEngine.Input.GetKeyDown(input.special))
                btn.valueDown += 4;
            if (UnityEngine.Input.GetKeyDown(input.guard))
                btn.valueDown += 8;

            if (UnityEngine.Input.GetKey(input.punch))
                btn.value += 1;
            if (UnityEngine.Input.GetKey(input.kick))
                btn.value += 2;
            if (UnityEngine.Input.GetKey(input.special))
                btn.value += 4;
            if (UnityEngine.Input.GetKey(input.guard))
                btn.value += 8;

            btnStream.RemoveAt(0);
            btnStream.Add(btn);

            InputEvent dir = new InputEvent(5);

            if (UnityEngine.Input.GetKeyDown(input.up))
                dir.valueDown += 3;
            if (UnityEngine.Input.GetKeyDown(input.down))
                dir.valueDown -= 3;
            if (UnityEngine.Input.GetKeyDown(input.left))
                dir.valueDown -= 1;
            if (UnityEngine.Input.GetKeyDown(input.right))
                btn.valueDown += 1;

            if (UnityEngine.Input.GetKey(input.up))
                dir.value += 3;
            if (UnityEngine.Input.GetKey(input.down))
                dir.value -= 3;
            if (UnityEngine.Input.GetKey(input.left))
                dir.value -= 1;
            if (UnityEngine.Input.GetKey(input.right))
                dir.value += 1;

            dirStream.RemoveAt(0);
            dirStream.Add(dir);
        }
        private void UpdateButtonInput()
        {
            int i = 0;

            if (UnityEngine.Input.GetKey(KeyCode.J))
                i += 1;
            if (UnityEngine.Input.GetKey(KeyCode.K))
                i += 2;
            if (UnityEngine.Input.GetKey(KeyCode.L))
                i += 4;
            if (UnityEngine.Input.GetKey(KeyCode.Semicolon))
                i += 8;

            string s = Convert.ToString(i, 2);
            PrintReadable(i);

            btnInput.Add(i);
            btnInput.RemoveAt(0);
        }
        private void UpdateDirInput(string dir)
        {
            dirInput = dirInput.Remove(0, 1) + dir;
        }
        private Vector2 GetDirectionalVector()
        {
            Vector2 input = Vector2.zero;

            if (UnityEngine.Input.GetKey(KeyCode.Space))
                input.y += 1;
            if (UnityEngine.Input.GetKey(KeyCode.S))
                input.y -= 1;
            if (UnityEngine.Input.GetKey(KeyCode.D))
                input.x += 1;
            if (UnityEngine.Input.GetKey(KeyCode.A))
                input.x -= 1;

            return input;
        }
        private string GetDirectionString(Vector3 dir)
        {
            int i = 5; // neutral

            // x
            if (dir.x > 0)
                i += 1;
            else if (dir.x < 0)
                i -= 1;

            // y
            if (dir.y > 0)
                i += 3;
            else if (dir.y < 0)
                i -= 3;

            return i.ToString();
        }

        // parsing input
        private void ParseCommandList()
        {
            if (btnInput[btnInput.Count - btnBuffer] != 0)
            {
                foreach (var cmd in cmdList)
                {
                    if (IsValidButton(cmd) && IsValidDirection(cmd))
                    {
                        print(cmd.name);
                        return;
                    }
                }
            }
        }
        private bool IsValidDirection(Command cmd)
        {
            // require directions in sequence, ignore neutral
            int position = cmd.directions.Length - 1;
            var searchChar = '-';
            for (int i = dirInput.Length - 1; i >= 0; i--)
            {
                // all positions have been validated
                if (position < 0)
                    return true;

                // skip neutral and repeated inputs
                while (i > 0 && (dirInput[i] == searchChar || dirInput[i] == '5'))
                    i--;

                // validate char at cmd position
                searchChar = cmd.directions[position];
                if (dirInput[i] == searchChar)
                    position--;
            }
            return false;
        }
        private bool IsValidButton(Command cmd)
        {
            // find most significant digit of command button
            int digit = (int)Mathf.Log(cmd.button);
            for (int i = 0; i <= digit; i++)
            {
                // fail if command requires input not being pressed
                if (IsBitOn(cmd.button, i) &&
                    !IsBitOn(btnInput[btnInput.Count - 1], i))
                    return false;
            }
            return true;
        }

        private void ParseEvents()
        {
            if (btnStream[btnInput.Count - btnBuffer].valueDown != 0)
            {
                foreach (var cmd in cmdList)
                {
                    if (IsValidBtnEvent(cmd) && IsValidDirEvent(cmd))
                    {
                        print(cmd.name);
                        return;
                    }
                }
            }
        }
        private bool IsValidDirEvent(Command cmd)
        {
            // require directions in sequence, ignore neutral
            int position = cmd.directions.Length - 1;
            int searchValue = 0;
            for (int i = dirStream.Count - 1; i >= 0; i--)
            {
                // all positions have been validated
                if (position < 0)
                    return true;

                // skip neutral and repeated inputs
                while (i > 0 && (dirStream[i].value == searchValue
                    || dirStream[i].value == 5))
                    i--;

                // validate char at cmd position
                searchValue = int.Parse(cmd.directions[position].ToString());
                if (dirStream[i].value == searchValue)
                    position--;
            }
            return false;
        }
        private bool IsValidBtnEvent(Command cmd)
        {
            // find most significant digit of command button
            int digits = (int)Mathf.Log(cmd.button);
            for (int i = 0; i <= digits; i++)
            {
                // fail if command requires input not being pressed
                if (IsBitOn(cmd.button, i)
                    && !IsBitOn(btnStream[btnStream.Count - 1].value, i))
                    return false;
            }
            return true;
        }

        private void PrintReadable(int input)
        {
            // https://stackoverflow.com/questions/34946568/finding-whether-bit-position-is-set-in-c-sharp

            string output = "----";

            for (int i = 0; i < 4; i++)
            {
                if (IsBitOnMod(input, i))
                {
                    var c = output.ToCharArray(0, 4);
                    c[i] = 'o';
                    output = new string(c);
                }
            }

            print(output);
        }

        // accomplish the same thing but mod came from my own brain
        private bool IsBitOn(int value, int pos)
        {
            return (value & (1 << pos)) != 0;
        }
        private bool IsBitOnMod(int value, int pos)
        {
            int lhs = (int)Math.Pow(2, pos + 1);
            int rhs = (int)Math.Pow(2, pos) - 1;
            return value % lhs > rhs;
        }

        private void CreateList()
        {
            cmdList = new Command[]
            {
                new Command("DP", 1, "623"),
                new Command("Grab", 1 + 2, "")
            };
        }
    }
}