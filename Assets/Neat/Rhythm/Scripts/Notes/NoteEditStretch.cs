using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Music
{
    public class NoteEditStretch : NoteEdit
    {
        private NoteUI ui;
        private TimeSpan oldSpan;
        private float newTime;

        public NoteEditStretch(NoteUI nui, float time)
        {
            this.ui = nui;
            this.newTime = time;
            this.oldSpan = new TimeSpan(nui.note.timeSpan);

            Execute();
        }

        public static bool isValid(NoteUI ui, float time)
        {
            // has UI and is outside time
            return ui != null && !ui.note.timeSpan.Contains(time);
        }
        public static void HandleNoteStretch(NoteUI ui, float time)
        {
            if (isValid(ui, time))
                new NoteEditStretch(ui, time);
        }

        public override void Execute()
        {
            Debug.Log("Execute note stretch");

            var timeSpan = ui.note.timeSpan;
            var timingMap = ui.overlay.controller.chart.timingMap;

            if (newTime < timeSpan.on)
            {
                var prev = timingMap.Earliest(timeSpan.on).Prev();
                ui.note.timeSpan.on = prev.time;
            }
            else if (newTime > timeSpan.off)
            {
                var next = timingMap.Next(timeSpan.on).Next();
                ui.note.timeSpan.off = next.time;
            }

            ui.UpdateUI();
            // base.Execute();
        }
        public override void Undo()
        {
            Debug.Log("Undo note stretch");

            ui.note.timeSpan = oldSpan;
            ui.UpdateUI();
        }
    }
}
