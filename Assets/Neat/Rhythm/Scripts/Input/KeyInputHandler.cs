using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Neat.Audio.Music
{
    // OverlayKeyInputHandler
    public class KeyInputHandler : UIEventHandler
    {
        private NoteUI _ui;
        public NoteUI ui;

        private void Awake()
        {
            ui = GetComponentInParent<NoteUI>();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            // create note from overlay
            var spawner = ui.overlay.noteSpawner;

            // create new timespan and add to notemap?
            spawner.Spawn(ui.note, true, true);
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
