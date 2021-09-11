using Neat.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Neat.Music
{
    using Input = UnityEngine.Input;
    using KeyCode = UnityEngine.KeyCode;
    public class NoteInputHandler : UIEventHandler, InputState
    {
        // this class shouldn't be handling logic, just routing events
        // use states for logic handling
        private ChartController _controller;
        public ChartController controller
        {
            get
            {
                if (_controller == null)
                    _controller = ui.overlay.controller;
                return _controller;
            }
        }
        private KeyOverlay _overlay;
        public KeyOverlay overlay
        {
            get
            {
                if (_overlay == null)
                    _overlay = ui.overlay;
                return _overlay;
            }
        }
        private NoteUI _ui;
        public NoteUI ui
        {
            get
            {
                if (!_ui)
                    _ui = GetComponentInParent<NoteUI>();
                return _ui;
            }
        }

        // later: multi-select support
        public static NoteUI selected;

        // passive input state
        public override void OnDrag(PointerEventData eventData)
        {
            // use this logic for hovering
            print("Mouse @ " + Input.mousePosition);
            print("Rect @ " + ui.rect.position + " (" + ui.rect.sizeDelta + ")");

            // if the mouse leaves the note rect...

            // to the left or right - extend to timing
            // up or down - change string

            var mouse = Input.mousePosition;
            if (mouse.y > ui.rect.position.y)
            {
                // change string up
            }
            else if (mouse.y < ui.rect.position.y + ui.rect.sizeDelta.y)
            {
                // change string down
            }
            else if (mouse.x < ui.rect.position.x)
            {
                // set note start to previous timing
            }
            else if (mouse.x > ui.rect.position.x + ui.rect.sizeDelta.x)
            {
                // set note end to next timing
            }
        }

        // drag state?
        private void StringUp()
        {
            int maxLane = 5; // notes don't have this reference
            bool hasNote = false; // is there already a note there?

            if (ui.note.lane < maxLane) // there is a higher string
            {
                ui.note.lane += 1;
            }
            //else if () // there is already a note there
            //{
            //}


            ui.note.lane += 1;
        }
        private void AdjustTimingBack()
        {

        }
        private void AdjustTimingForward()
        {

        }

        // click state?
        public override void OnPointerDown(PointerEventData eventData)
        {

        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            print("Clicked " + ui.note.FullFullName());
            Select();
        }


        // selected state?
        public void GetInput()
        {
            // click outside to cancel changes
            if (Input.GetMouseButtonDown(0))
            {
                var selected = EventSystem.current.currentSelectedGameObject;
                if (selected != this.gameObject)
                {
                    print("Cancelling edit...");
                    Deselect();
                }
            }

            // right click to delete (delete without being selected?)
            if (Input.GetMouseButtonDown(1))
            {
                var selected = EventSystem.current.currentSelectedGameObject;

                if (selected == this.gameObject)
                {
                    Deselect();

                    // tell the track
                    Destroy(ui.gameObject);
                }
            }

            // enter to confirm changes
            if (Input.GetKeyDown(KeyCode.Return))
            {
                print("Applying...");
                Deselect();
            }


            // change guitar fret on mouse scroll
            HandleMouseWheel();
        }
        private void HandleMouseWheel()
        {
            // scrollstate.scroll

            // or left-right keys
            var wheel = Input.mouseScrollDelta.y;
            if (Mathf.Abs(wheel) > .1f)
            {
                FretScroll((int)wheel);
            }
        }
        private void FretScroll(int value)
        {
            // scrollstate.scroll

            // feeling like this logic shouldn't be here
            int newFret = ui.note.fret - value;
            newFret = Mathf.Clamp(newFret, 0, 24);

            int delta = newFret - ui.note.fret;
            ui.note.fret += delta;
            ui.note.value += delta;

            ui.UpdateText();
        }

        private void PrintMousePosition()
        {
            print("Mouse @ " + Input.mousePosition);
        }
        public void Select()
        {
            print("Selecting " + ui.note.FullFullName());

            Deselect();
            selected = ui;
            controller.states.SetInput(this);
            ui.ToggleHighlight(true);

        }
        public static void Deselect()
        {
            // deselect current
            if (selected != null)
            {
                print("Deselecting " + selected.gameObject.name);
                selected.overlay.controller.states.UpdateInput();
                selected.ToggleHighlight(false);
            }
        }
    }
}
