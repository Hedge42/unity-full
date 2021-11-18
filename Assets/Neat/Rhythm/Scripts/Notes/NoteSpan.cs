using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Neat.Music
{
    // this class is to prevent having to recalculate what notes
    // are in the controller's timespan
    public class NoteSpan
    {
        private ChartPlayer player;

        public List<Note> notes { get; private set; } // incoming?
        public List<Note> playing { get; private set; }

        public event Action<Note> onNoteAppear;
        public event Action<Note> onNoteDisappear;

        // ChartController.onNoteOn
        // controller.windowNoteSpan
        // controller.playingNoteSpan
        public UnityEvent<Note> onNoteOn;
        public UnityEvent<Note> onNoteOff;

        private Note Next()
        {
            if (notes.Count > 0)
            {
                var lastNote = notes[notes.Count - 1];
                return player.noteMap.Next(lastNote);
            }
            else
            {
                return player.noteMap.Next(player.time);
            }
        }
        private Note first
        {
            get
            {
                if (notes.Count > 0)
                    return notes[0];
                else
                    return Next();
            }
        }

        private Note _recent;
        public Note recent
        {
            get
            {
                if (_recent == null)
                {
                    var prev = player.noteMap.notes.Where(n => n.timeSpan.on <= player.time);
                    // https://stackoverflow.com/questions/1101841/how-to-perform-max-on-a-property-of-all-objects-in-a-collection-and-return-th
                    // var maxObject = list.OrderByDescending(item => item.Height).First();
                    _recent = prev.OrderByDescending(n => n.timeSpan.on).FirstOrDefault();
                }
                return _recent;
            }
        }
        public Note Recent()
        {
            var prev = player.noteMap.notes.Where(n => n.timeSpan.on <= player.time);
            // https://stackoverflow.com/questions/1101841/how-to-perform-max-on-a-property-of-all-objects-in-a-collection-and-return-th
            // var maxObject = list.OrderByDescending(item => item.Height).First();
            _recent = prev.OrderByDescending(n => n.timeSpan.on).FirstOrDefault();
            return _recent;
        }

        public NoteSpan(ChartPlayer player)
        {
            onNoteOn = new UnityEvent<Note>();
            onNoteOff = new UnityEvent<Note>();

            playing = new List<Note>();

            this.player = player;
            Calculate(0f);

            player.player.onSkip.AddListener(Calculate);
            player.player.onTick.AddListener(Update);
        }

        private void Calculate(float _)
        {
            notes = player.noteMap.GetNotes(player.timer.fullTimespan);
            UpdatePlaying();
        }

        public void Update(float _)
        {
            // while there is a next note and it's time is visible
            var next = Next();
            if (next != null && next.timeSpan.on < player.timer.maxTime)
            {
                notes.Add(next);
                onNoteAppear?.Invoke(next);
                next = Next();

                // Debug.Log("Adding next...");
            }

            if (first != null && first.timeSpan.off < player.timer.minTime)
            {
                onNoteDisappear?.Invoke(first);
                notes.RemoveAt(0);

                // Debug.Log("Adding next...");
            }

            UpdatePlaying();
        }

        private void UpdateRecent()
        {
            var next = player.noteMap.Next(recent);
            while (next.timeSpan.on <= player.time)
            {
                _recent = next;
                next = player.noteMap.Next(next);
            }
        }

        private void UpdatePlaying()
        {
            // slow

            // send note-off events
            var toRemove = new List<Note>();
            foreach (var n in playing)
            {
                if (n.timeSpan.off < player.time)
                {
                    toRemove.Add(n);
                }
            }
            foreach (var n in toRemove)
            {
                playing.Remove(n);
                onNoteOff?.Invoke(n);
            }

            // var _recent = notes.Where(n => n.timeSpan.off < player.time)
            // send note-on events
            var _playing = notes.Where(n => n.timeSpan.Contains(player.time)).ToList();

            // new, except old
            var toAdd = _playing.Except(playing);
            foreach (var n in toAdd)
            {
                playing.Add(n);
                onNoteOn?.Invoke(n);
            }


            //// get next note?
            //if (playing.Count == 0)
            //{
            //    var next = player.noteMap.Next(player.time);

            //    if (next.timeSpan.Contains())
            //}
            //else
            //{
            //    var next = player.noteMap.Next(playing.Last());
            //}
        }
    }
}
