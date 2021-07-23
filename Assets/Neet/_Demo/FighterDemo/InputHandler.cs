using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Fighter
{
    public class InputHandler : MonoBehaviour
    {
        private List<InputEvent> inputStream;

        public InputSetting inputSetting;

        private FighterMotor motor;

        private Character c;

        // private Command[] cmdList;

        private void Start()
        {
            Time.fixedDeltaTime = 1f / 60f;

            SetupInputStream();

            // cmdList = new MoveListStandard().commands;

            motor = GetComponent<FighterMotor>();
            c = GetComponent<CharacterSelectorComponent>().character;
        }

        private void FixedUpdate()
        {
            UpdateInput();
            motor.Walk(c, inputStream[inputStream.Count - 1]);
        }

        private void SetupInputStream()
        {
            inputStream = new List<InputEvent>();
            for (int i = 0; i < Fighter.INPUT_STREAM_LENGTH; i++)
                inputStream.Add(new InputEvent());
        }

        private void UpdateInput()
        {
            InputEvent i = new InputEvent();

            if (UnityEngine.Input.GetKeyDown(inputSetting.punch))
                i.btnDown += Fighter.P_VALUE;
            if (UnityEngine.Input.GetKeyDown(inputSetting.kick))
                i.btnDown += Fighter.K_VALUE;
            if (UnityEngine.Input.GetKeyDown(inputSetting.special))
                i.btnDown += Fighter.S_VALUE;
            if (UnityEngine.Input.GetKeyDown(inputSetting.guard))
                i.btnDown += Fighter.G_VALUE;

            if (UnityEngine.Input.GetKey(inputSetting.punch))
                i.btn += Fighter.P_VALUE;
            if (UnityEngine.Input.GetKey(inputSetting.kick))
                i.btn += Fighter.K_VALUE;
            if (UnityEngine.Input.GetKey(inputSetting.special))
                i.btn += Fighter.S_VALUE;
            if (UnityEngine.Input.GetKey(inputSetting.guard))
                i.btn += Fighter.G_VALUE;

            if (UnityEngine.Input.GetKeyDown(inputSetting.up))
                i.dirDown += 3;
            if (UnityEngine.Input.GetKeyDown(inputSetting.down))
                i.dirDown -= 3;
            if (UnityEngine.Input.GetKeyDown(inputSetting.left))
                i.dirDown -= 1;
            if (UnityEngine.Input.GetKeyDown(inputSetting.right))
                i.dirDown += 1;
            i.dirDownBit = Fighter.DirToBitValue(i.dirDown);

            if (UnityEngine.Input.GetKey(inputSetting.up))
                i.dir += 3;
            if (UnityEngine.Input.GetKey(inputSetting.down))
                i.dir -= 3;
            if (UnityEngine.Input.GetKey(inputSetting.left))
                i.dir -= 1;
            if (UnityEngine.Input.GetKey(inputSetting.right))
                i.dir += 1;
            i.dirBit = Fighter.DirToBitValue(i.dir);

            inputStream.Add(i);
            inputStream.RemoveAt(0);
        }

        
        private string ShowButtonBits()
        {
            int btn = inputStream[inputStream.Count - 1].btn;

            string s = "";
            for (int i = 0; i < 4; i++)
            {
                if (btn.IsBitOn(i))
                    s = Fighter.BitToButtonString(i) + s;
                else
                    s = "-" + s;
            }

            return "BTN: " + s;
        }
        private string ShowDirectionBits()
        {
            int dirBit = inputStream[inputStream.Count - 1].dirBit;

            string s = "";

            if (dirBit.IsBitOn(Fighter.UP_BIT))
                s = "↑" + s;
            else
                s = "o" + s;

            if (dirBit.IsBitOn(Fighter.DOWN_BIT))
                s = "↓" + s;
            else
                s = "o" + s;

            if (dirBit.IsBitOn(Fighter.LEFT_BIT))
                s = "←" + s;
            else
                s = "o" + s;

            if (dirBit.IsBitOn(Fighter.RIGHT_BIT))
                s = "→" + s;
            else
                s = "o" + s;

            return "DIR: " + s;
        }
    }
}