using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Music
{
    [ExecuteAlways]
    public class FretboardNoteReader : MonoBehaviour, Loadable
    {
        [SerializeField] private Fretboard fretboard;
        [SerializeField] private ChartPlayer player;

        public void SetTime(float newTime)
        {
            // var notes = player.noteMap.GetNotes(newTime - player.ui.scroller.approachRate, player.ui.scroller.maxTime);

            var notes = player.noteMap.GetNotes(player.timer.fullTimespan);
            Refresh(notes);
        }
        public void UpdateTime(float newTime)
        {
            SetTime(newTime);
            // when a note appears in the range,
            // trigger the corresponding fret's fade-in animation

            // for each note in the active range
            // set the corresponding fret's animation state


        }

        private void Refresh(List<Note> notes)
        {
            print("Reading notes for fretboard...");
            foreach (var n in notes)
            {
                // find the corresponding fret animator
                var anim = fretboard.GetFret(n).animator;
                anim.SetAnimationState(n, player);
            }
        }

        public void OnLoad(Chart c)
        {
            player.player.onSkip.AddListener(SetTime);
            player.player.onTick.AddListener(UpdateTime);

            FormatFretDisplayMode();

            SetTime(0f);
        }

        private void FormatFretDisplayMode()
        {
            foreach (Fret f in fretboard.frets)
            {
                f.displayMode = Fret.FretToggleMode.Normal;
                f.mono.tmp.color = Color.clear;
            }
        }
    }

    public class NoteReader
    {
        private List<Note> active;
        private ChartPlayer player;

        private Note waiting;

        public NoteReader(ChartPlayer player)
        {
            this.player = player;
        }

        // find notes
        public void SetTime(float newTime)
        {
            active = player.noteMap.GetNotes(player.timer.fullTimespan);
        }

        // update notes
        public void UpdateTime(float newTime)
        {
            // removes notes that are already off
            while (active.Count > 0 && active[0].timeSpan.off < player.time)
                active.RemoveAt(0);

            // spawn notes that are now visible
            while (HasNext(out Note next))
                active.Add(next);
        }
        private bool HasNext(out Note next)
        {
            next = player.noteMap.Next(active[active.Count - 1]);
            bool hasNext = next != null;
            bool spawnNext = hasNext && next.timeSpan.on < player.timer.maxTime;
            return spawnNext;
        }
    }
}
