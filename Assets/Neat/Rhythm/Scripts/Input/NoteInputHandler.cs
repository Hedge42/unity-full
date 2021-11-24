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
        public static readonly List<int> ex = new List<int>();

        // this class shouldn't be handling logic, just routing events
        // use states or other classes for logic
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

        //private NoteScrollHover _hover;
        //public NoteScrollHover hover => _hover == null ? _hover = new NoteScrollHover() : _hover;
        private HighwayInputHandler _hInput;
        private HighwayInputHandler hInput => _hInput == null ? _hInput = controller.GetComponent<HighwayInputHandler>() : _hInput;

        //private HighwayInputHandler hInput
        //{
        //    get
        //    {
        //        if (_hInput == null)
        //            _hInput = controller.GetComponent<HighwayInputHandler>();

        //        if (_hInput == null)
        //            print("oh no!");

        //        return _hInput;
        //    }
        //}

        private Vector2 mouseDownPos;

        // later: multi-select support
        public static NoteUI selected;

        private Timing mouseDownTiming;


        public void GetInput()
        {
            // click outside to cancel changes
            if (Input.GetMouseButtonUp(0))
            {
                // execute note change event
                Deselect();
            }

            // enter to confirm changes
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                Deselect();
            }


            // change guitar fret on mouse scroll
            HandleMouseWheel();
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                SetMouseStartPosition();
                Select(); // this is a command
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                Delete(); // this is a command
            }
        }
        public override void OnDrag(PointerEventData eventData)
        {
            // dont need to use this drag...

            // print("Hovered Timing: " + HoveredTiming().ToString());
            // print("Hovered Lane: " + HoveredNote().ToString());
            // Vector2 dragDelta = (Vector2)Input.mousePosition - mouseDownPos;

            bool timing = TimingDrag();
            bool note = NoteDrag();

            if (timing || note)
            {
                // MakeChange();
            }
        }

        private bool IsGameObjectSelected()
        {

            var selected = EventSystem.current.currentSelectedGameObject;
            return selected != this.gameObject;
        }
        private void Delete()
        {
            Destroyer.Destroy(ui.gameObject);
            controller.noteMap.Remove(ui.note);
            controller.player.SkipTo(controller.time);
        }
        private void SetMouseStartPosition()
        {
            mouseDownPos = Input.mousePosition;
            mouseDownTiming = hInput.hover.HoveredTiming(mouseDownPos);
        }

        private void MakeChange()
        {
            SetMouseStartPosition();
            ui.UpdateUI();
            ui.Select(true);
        }
        private bool NoteDrag()
        {
            var _note = hInput.hover.PotentialNote();
            if (_note.lane > ui.note.lane)
            {
                // drag up
                StringUp();
                return true;
            }
            else if (_note.lane < ui.note.lane)
            {
                // drag down
                StringDown();
                return true;
            }
            else
                return false;
        }
        private void StringUp()
        {
            bool hasNote = false; // is there already a note there?
            int lane = Mathf.Clamp(ui.note.lane + 1, 0, overlay.numLanes - 1);

            if (!hasNote && ui.note.lane < overlay.numLanes - 1) // there is a higher string
            {
                //print("Edit string up...");

                ui.note.lane += 1;

                // default → update note
                // shift → update fret
                if (!Input.GetKey(KeyCode.LeftShift))
                    ui.note.UpdateNote();
                else
                    ui.note.UpdateFret();

                MakeChange();
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
                //print("Edit string down...");

                ui.note.lane -= 1;

                // default → update note
                // shift → update fret
                if (!Input.GetKey(KeyCode.LeftShift))
                    ui.note.UpdateNote();
                else
                    ui.note.UpdateFret();

                MakeChange();
            }
            else
            {
                print("Can't string down");
            }
        }

        private bool TimingDrag()
        {
            var _timing = hInput.hover.HoveredTiming(Input.mousePosition);
            if (_timing.Equals(mouseDownTiming.Next()))
            {
                // drag right
                TimingForward();
                return true;
            }
            else if (_timing.Equals(mouseDownTiming.Prev()))
            {
                // drag left
                TimingBack();
                return true;
            }
            else
                return false;
        }
        private void TimingBack()
        {
            var prev = mouseDownTiming.Prev();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                // drag note end
                if (mouseDownTiming.time > ui.note.timeSpan.on)
                    ui.note.timeSpan.off = mouseDownTiming.time;
            }
            else
            {
                var duration = ui.note.timeSpan.duration;
                ui.note.timeSpan = new TimeSpan(prev.time, prev.time + duration);
            }

            MakeChange();
        }
        private void TimingForward()
        {
            var next = mouseDownTiming.Next();
            var duration = ui.note.timeSpan.duration;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                ui.note.timeSpan.off = next.Next().time;
            }
            else
            {
                ui.note.timeSpan.on = next.time;
                ui.note.timeSpan.off = next.time + duration;
            }

            MakeChange();
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


        // commands



        private void HandleMouseWheel()
        {
            // or left-right keys ???
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

        // debugging
        private void PrintMousePosition()
        {
            print("Mouse @ " + Input.mousePosition);
        }
        private static void printBounds(float top, float bottom, float left, float right)
        {
            print("Bounds: \ntop: " + top
                            + "\nbottom: " + bottom
                            + "\nleft: " + left
                            + "\nright: " + right
                            );
        }
    }
}
