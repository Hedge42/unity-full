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
            // print("Overlay note clicked " + ui.note.Name());
            ui.overlay.track.CreateTrackNote(ui.note);
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
