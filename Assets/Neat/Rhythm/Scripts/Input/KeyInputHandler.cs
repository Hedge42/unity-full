using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Neat.Music
{
    public class KeyInputHandler : UIEventHandler
    {
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

        public override void OnPointerClick(PointerEventData eventData)
        {
            // create track note from overlay
            print("Overlay note clicked " + ui.note.FullName());
            float duration = GetDuration();
            Note note = SetTimings(duration);

            ui.overlay.track.CreateTrackNote(note);
        }

        private Note SetTimings(float duration)
        {
            // clone and set timings
            var note = ui.note.Clone();
            note.on = ui.overlay.controller.time;
            note.off = note.on + duration;
            return note;
        }

        private float GetDuration()
        {
            // find length of note
            var controller = ui.overlay.controller;
            var timingMap = controller.chart.timingMap;
            TimeSignature signature = timingMap.GetSignatureAtTime(controller.time);
            float duration = 0f;
            if (signature != null)
                duration = signature.TimePerDivision(signature.denominator);
            else
                duration = Mathf.Min(1f, timingMap.
                    Earliest(controller.time).time - controller.time);
            return duration;
        }


        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            //print("Dragging?");
            //base.OnInitializePotentialDrag(eventData);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            //print("Stopped dragging");
            //base.OnEndDrag(eventData);
        }
    }
}
