using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

// a monobehavior layer for fret/border objects
// serves as a template + container for fret and border objects
namespace Neat.Audio.Music
{
    public class FretUI : MonoBehaviour, IPointerClickHandler
    {
        public Fret fret;

        public TextMeshProUGUI tmp;

        public Image fill;
        public Image borderDot1;
        public Image borderDot2;

        public Fret.FretToggleMode displayMode
        {
            get { return fret.displayMode; }
            set { fret.displayMode = value; }
        }
        public Fretboard fretboard
        {
            get { return fret.fretboardUI; }
            set { fret.fretboardUI = value; }
        }
        public int fretNum
        {
            get { return fret.fretNum; }
            set { fret.fretNum = value; }
        }

        public void UpdateText()
        {
            fret.UpdateDisplay();
        }
        public void Hide()
        {
            fret.Hide();
        }
        public void ToggleMode()
        {
            fret.ToggleMode();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            print("Fret " + fret.stringNum + "[" + fret.fretNum + "] clicked");
            if (eventData.button == PointerEventData.InputButton.Right)
                AltClick();
            else if (eventData.button == PointerEventData.InputButton.Left)
                Click();
        }

        private void NotifyFretboard()
        {
            // stinky

            // tell the fretboard this was clicked
            if (fret is FretObject)
                fretboard.FretClickedHandler(fret.fretNum, fret.stringNum);
        }

        private void Click()
        {
            print("Clicked " + fret.ToString());
            fret.ToggleMode();
            NotifyFretboard();
        }
        private void AltClick()
        {
            print("AltClicked " + fret.ToString());
        }
    }
}