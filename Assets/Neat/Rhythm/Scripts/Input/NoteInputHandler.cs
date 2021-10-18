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
        private ChartPlayer _controller;
        public ChartPlayer controller
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

        // use this for drag
        private Vector2 mouseDownPos;

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

            // center origin?
            if (mouse.y > ui.rect.position.y + ui.rect.sizeDelta.y)
            {
                // change string up
                StringUp();
            }
            else if (mouse.y < ui.rect.position.y - ui.rect.sizeDelta.y)
            {
                // change string down
                StringDown();
            }
            else if (mouse.x < ui.rect.position.x - ui.rect.sizeDelta.x / 2)
            {
                // set note start to previous timing
                TimingBack();
            }
            else if (mouse.x > ui.rect.position.x + ui.rect.sizeDelta.x)
            {
                // set note end to next timing
                TimingForward();
            }
        }

        // drag state?
        private void StringUp()
        {
            bool hasNote = false; // is there already a note there?

            if (!hasNote && ui.note.lane < overlay.numLanes - 1) // there is a higher string
            {
                ui.note.lane += 1;

                // default → update note
                // shift → update fret
                if (!Input.GetKey(KeyCode.LeftShift))
                    ui.note.UpdateNote();
                else
                    ui.note.UpdateFret();

                ui.UpdateUI();
            }
            else
            {
                print("Can't string up");
            }
        }
        private void StringDown()
        {
            bool hasNote = false; // is there already a note there?

            if (!hasNote && ui.note.lane >= 0) // there is a lower string
            {
                ui.note.lane -= 1;

                // default → update note
                // shift → update fret
                if (!Input.GetKey(KeyCode.LeftShift))
                    ui.note.UpdateNote();
                else
                    ui.note.UpdateFret();

                ui.UpdateUI();
            }
            else
            {
                print("Can't string down");
            }
        }

        private void TimingBack()
        {
            var earliest = controller.chart.timingMap.Earliest(ui.note.timeSpan.on);
            var prev = earliest.Prev();
            var duration = ui.note.timeSpan.duration;
            ui.note.timeSpan.on = prev.time;

            // stretch if holding shift
            if (!Input.GetKey(KeyCode.LeftShift))
                ui.note.timeSpan.off = prev.time + duration;

            ui.UpdateTransform();
        }
        private void TimingForward()
        {
            var earliest = controller.chart.timingMap.Earliest(ui.note.timeSpan.on);
            var next = earliest.Next();
            var nextX = next.time * overlay.controller.ui.scroller.distancePerSecond;

            var duration = ui.note.timeSpan.duration;

            // stretch if holding shift
            if (!Input.GetKey(KeyCode.LeftShift))
                ui.note.timeSpan.on = next.time;
            ui.note.timeSpan.off = next.time + duration;
            ui.UpdateTransform();
        }

        // click state?
        public override void OnPointerDown(PointerEventData eventData)
        {
            // start drag
            print("Pointer down " + ui.note.FullFullName());
            mouseDownPos = Input.mousePosition;
            Select();
        }

        // selected state?
        public void GetInput()
        {
            // click outside to cancel changes
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
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
            ui.Select(true);

        }
        public static void Deselect()
        {
            // deselect current
            if (selected != null)
            {
                print("Deselecting " + selected.gameObject.name);
                selected.overlay.controller.states.UpdateInput();
                selected.Select(false);
            }
        }
    }
}
