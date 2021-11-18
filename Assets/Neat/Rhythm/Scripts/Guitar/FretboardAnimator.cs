using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Music
{
    [ExecuteAlways]
    public class FretboardAnimator : MonoBehaviour, Loadable
    {
        [SerializeField] private Fretboard fretboard;

        [SerializeField] private ChartPlayer player;

        public Color incomingColor;
        public Color outgoingColor;
        public Color playingColor;
        public Color visibleColor;

        public AnimationCurve fadeInCurve;
        public AnimationCurve fadeOutCurve;

        public float fadeInTime => player.timer.approachRate;
        [Range(0f, 2f)] public float fadeOutTime;
        [Range(0, 20)] public float fadeInScale;
        [Range(0, 20)] public float fadeOutScale;

        private List<FretAnimation> _anims;
        private List<FretAnimation> anims
        {
            get
            {
                if (_anims == null)
                    _anims = new List<FretAnimation>();
                return _anims;
            }
        }


        // initializing
        public void OnLoad(Chart c)
        {
            SetEvents();
            SetTime(0f);
        }
        public void SetTime(float newTime)
        {
            // slow
            FormatFretboard();
            RespawnAnimations();

        }
        private void FormatFretboard()
        {
            foreach (Fret f in fretboard.playableFrets)
            {
                f.displayMode = Fret.FretToggleMode.Normal;
                f.mono.tmp.color = Color.clear; // disable text?
                f.UpdateDisplay();
            }
        }
        private void RespawnAnimations()
        {
            DestroyExisting();
            var notes = player.noteSpan.notes;
            foreach (Note note in notes)
                SpawnAnimation(note);
        }

        // updating
        public void UpdateTime(float newTime)
        {
            foreach (var anim in anims)
                anim.Update();
        }
        private void SpawnAnimation(Note n)
        {
            var fret = fretboard.GetFret(n).fret;
            anims.Add(new FretAnimation(this, fret, n, player));
        }

        // events
        private void SetEvents()
        {
            player.player.onSkip.AddListener(SetTime);
            player.player.onTick.AddListener(UpdateTime);
            player.noteSpan.onNoteAppear += NoteAppeared;
            player.noteSpan.onNoteDisappear += NoteDisappeared;
        }
        private void NoteAppeared(Note n)
        {
            SpawnAnimation(n);
        }
        private void NoteDisappeared(Note n)
        {
            if (anims.Count >= 1)
            {
                anims[0].Destroy();
                anims.RemoveAt(0); // ????
            }
        }

        private void DestroyExisting()
        {
            foreach (var anim in anims)
                anim.Destroy();

            anims.Clear();
        }
    }
}
