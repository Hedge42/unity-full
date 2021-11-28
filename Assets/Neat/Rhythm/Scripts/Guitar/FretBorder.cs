using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Audio.Music
{
    public class BorderObject : Fret
    {
        public BorderObject(FretUI prefab, Transform container, Fretboard fretboardUI, int fretNum)
        {
            var mono = GameObject.Instantiate(prefab, container);
            mono.gameObject.SetActive(true);
            mono.fret = (Fret)this;

            this.rect = mono.GetComponent<RectTransform>();
            this.mono = mono;
            this.fretNum = fretNum;
            this.fretboardUI = fretboardUI;
        }

        public override void ToggleMode()
        {
            base.ToggleMode();
        }

        public override void UpdateDisplay()
        {
            var setting = fretboardUI.displaySetting;
            var isDots = setting.borderMode == Fret.BorderMode.Dots;
            var isNumbers = setting.borderMode == Fret.BorderMode.Numbers;

            var isHidden = displayMode == FretToggleMode.Hidden;
            var isNormal = displayMode == FretToggleMode.Normal;
            var isEmphasized = displayMode == FretToggleMode.Emphasized;


            mono.tmp.gameObject.SetActive(!isHidden && isNumbers);
            mono.borderDot1.gameObject.SetActive(!isHidden && isDots);
            mono.borderDot2.gameObject.SetActive(!isHidden && isDots && isEmphasized);

            if (!isHidden && isNumbers)
            {
                string s = fretNum.ToString();
                mono.tmp.text = s;

                if (isEmphasized)
                    mono.tmp.fontStyle = TMPro.FontStyles.Bold;
                else
                    mono.tmp.fontStyle = TMPro.FontStyles.Normal;
            }

        }
    }
}