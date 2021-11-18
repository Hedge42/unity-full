using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat.Music
{
    public class NoteEditShrink : NoteEdit
    {
        private NoteUI ui;
        private TimeSpan oldSpan;
        private float newTime;

        public NoteEditShrink(NoteUI nui, float time)
        {
            this.ui = nui;
            this.newTime = time;
            this.oldSpan = new TimeSpan(nui.note.timeSpan);

            Execute();
        }
        public static void HandleNoteShrink(NoteUI ui, float time)
        {
            if (isValid(ui, time))
                new NoteEditShrink(ui, time);
        }
        public static bool isValid(NoteUI ui, float time)
        {
            // has UI and is outside time
            return ui != null && ui.note.timeSpan.Contains(time);
        }
        public override void Execute()
        {
            base.Execute();
        }
    }
}
