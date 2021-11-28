using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Demos.Fighter
{
    public class Command
    {
        public string name { get; private set; }
        public string input { get; private set; }
        public string dirInput { get; private set; }
        public string btnInput { get; private set; }
        private bool isBtnInput { get; set; }
        private bool isDirInput { get; set; }

        public List<DirRequirement> dirReqs { get; private set; }
        public BtnRequirement btnReq { get; private set; }

        public Command(string _name, string _input)
        {
            this.name = _name;
            this.input = _input;

            GetRequirements(_input);
        }

        public bool IsValid(List<InputEvent> inputs)
        {
            return IsBtnValid(inputs) && IsDirValid(inputs);
        }

        private bool IsBtnValid(List<InputEvent> inputList)
        {
            if (!isBtnInput)
                return true;

            var br = btnReq;

            int bufferedBtnDown = 0;
            int startIndex = inputList.Count - Fighter.BTN_BUFFER;

            for (int i = startIndex; i < inputList.Count; i++)
            {
                // earliest buffer frame must include part of btnDown requirement
                if (i == startIndex)
                {
                    if (!br.btnDown.ContainsBits(inputList[startIndex].btnDown))
                        return false;
                }

                // holdable buttons must be held while down buttons are pressed
                bool isHold = inputList[i].btn.ContainsBits(br.btnHoldable);
                if (isHold)
                    bufferedBtnDown |= inputList[i].btnDown;
            }

            return bufferedBtnDown.ContainsBits(br.btnDown);
        }
        private bool IsDirValid(List<InputEvent> inputList)
        {
            if (!isDirInput)
                return true;

            var dr = dirReqs;
            int pos = dr.Count - 1;

            int previousDir = 0;

            // shouldn't validate directions after earliest buffer 
            for (int i = inputList.Count - Fighter.BTN_BUFFER; i > 0; i--)
            {
                var e = inputList[i];

                // skip repeated inputs
                if (previousDir == e.dirBit)
                    continue;

                // shift position on match
                if (ValidatePosition(e, dr[pos]))
                    pos -= 1;

                // check previous requirement and step over optional if successful
                else if (dr[pos].isOptional && ValidatePosition(e, dr[pos - 1]))
                    pos -= 2;

                // all positions validated
                if (pos < 0)
                    return true;
            }

            return false;
        }

        private bool ValidatePosition(InputEvent i, DirRequirement dr)
        {
            if (dr.isHoldable)
                // input has any bit of requirement
                return dr.dir.ContainsBits(i.dirBit);
            else
                return dr.dir.ContainsBits(i.dirDownBit);
        }

        /// <summary>
        /// Creates data-oriented command events at start <br/>
        /// to prevent having to continually parse strings while playing
        /// </summary>
        private void GetRequirements(string _input)
        {
            string[] _dirBtn = SplitDirBtn(_input);
            string _dirInput = _dirBtn[0];
            string _btnInput = _dirBtn[1];

            dirReqs = new List<DirRequirement>();
            string[] dirs = _dirInput.Split(',');
            foreach (string _dir in dirs)
                dirReqs.Add(new DirRequirement(_dir));

            btnReq = new BtnRequirement(_btnInput);
            isBtnInput = _btnInput.Length > 0;
            isDirInput = _dirInput.Length > 0;
        }

        /// <summary>
        /// Returns array [direction, button]
        /// </summary>
        private static string[] SplitDirBtn(string s)
        {
            string[] arr = new string[2];

            // find button start index
            int btnStart;
            for (btnStart = 0; btnStart < s.Length; btnStart++)
            {
                if (s[btnStart] == 'p' || s[btnStart] == 'k'
                    || s[btnStart] == 's' || s[btnStart] == 'g')
                {
                    break;
                }
            }

            // handle missing button
            if (btnStart < s.Length)
                arr[1] = s.Substring(btnStart);
            else
                arr[1] = "";
            arr[0] = s.Substring(0, btnStart);

            return arr;
        }

        /// <summary>
        /// Looks to see if next character in string is a button-modifier
        /// </summary>
        private static bool HasModifier(string s, int i)
        {
            if (i + 1 < s.Length)
            {
                char c = s[i + 1];
                return c == '*' || c == '?' || c == '[' || c == ']';
            }
            else
                return false;
        }
    }
}
