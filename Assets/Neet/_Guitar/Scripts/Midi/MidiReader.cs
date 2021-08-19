using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Tools;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Devices;
using System.Threading;
using System.Collections;

namespace Neet.Guitar
{
    public class MidiReader : MonoBehaviour
    {
        // https://melanchall.github.io/drywetmidi/

        // data
        public string path;
        private MidiFile midiFile;
        private Playback playback;
        private OutputDevice output;
        private bool isPlaying;

        // mono
        private void Start()
        {
            playback = GetPlayback();
            SetErrorListener();
        }
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                if (isPlaying)
                    playback.Stop();
                else
                    Play();

                isPlaying = !isPlaying;
            }
        }
        private void OnDisable()
        {
            Dispose();
        }
        private void OnApplicationPause(bool pause)
        {
            if (pause)
                Dispose();
        }
        private void OnApplicationQuit()
        {
            Dispose();
        }

        // playback
        private Playback GetPlayback()
        {
            var midiFile = MidiFile.Read(path);



            //output = OutputDevice.GetByName("Microsoft GS Wavetable Synth");
            output = OutputDevice.GetById(0);

            playback = midiFile.GetPlayback(output, new PlaybackSettings()
            {
                ClockSettings = new MidiClockSettings()
                {
                    CreateTickGeneratorCallback = () => null
                }
            });

            return playback;
        }
        private IEnumerator _Play()
        {
            playback.Start();

            while (playback.IsRunning)
            {
                yield return null;
                playback.TickClock();
            }

            playback.Dispose();
            output.Dispose();

            print("here?");
        }
        public void Play()
        {
            StartCoroutine(_Play());
        }
        public void Stop()
        {
            playback.Stop();
        }

        private MidiFile LoadMidi()
        {
            this.midiFile = MidiFile.Read(path,
                new ReadingSettings
                {
                    NoHeaderChunkPolicy = NoHeaderChunkPolicy.Abort,
                    CustomChunkTypes = new ChunkTypesCollection
                    {
                        //{ typeof(MyCustomChunk), "Cstm" }
                    }
                });
            return midiFile;
        }
        private void Dispose()
        {
            try
            {
                playback.Stop();

                output.Dispose();
                playback.Dispose();

            }
            catch { }
        }

        private void SetErrorListener()
        {
            playback.DeviceErrorOccurred += delegate { print("oops"); Dispose(); };
        }

        public void OpenPath()
        {
            Neet.File.FileManager.OpenExplorer(path);
        }
        public void ReadFile()
        {
            LoadMidi();
            midiFile = midiFile.Clone();
            var tm = midiFile.GetTempoMap();

            ICollection<Melanchall.DryWetMidi.Interaction.Note> notes = midiFile.GetNotes();

            Quantize(notes, tm);
        }

        private void Quantize(ICollection<Melanchall.DryWetMidi.Interaction.Note> notes, TempoMap tm)
        {
            NotesQuantizer nq = new NotesQuantizer();
            var settings = new NotesQuantizingSettings();
            settings.FixOppositeEnd = true;
            var grid = new SteppedGrid(MusicalTimeSpan.Eighth);
            nq.Quantize(notes, grid, tm, settings);
            foreach (var n in notes)
                PrintNote(n, tm);
        }

        private void PrintNote(Melanchall.DryWetMidi.Interaction.Note s, TempoMap tm)
        {
            print("note(" + s.NoteNumber + ") "
                    + "metricTime(" + s.TimeAs(TimeSpanType.Metric, tm) + ") "
                    + "metricLength(" + s.LengthAs<MetricTimeSpan>(tm) + ") \n"
                    + "barBeatTime(" + s.TimeAs<BarBeatFractionTimeSpan>(tm) + ") "
                    + "barBeatLength(" + s.LengthAs<BarBeatFractionTimeSpan>(tm) + ") "
                    );

            s.TimeAs<BarBeatFractionTimeSpan>(midiFile.GetTempoMap());
        }


        void WriteMidi()
        {
            midiFile.Write("My Great Song.mid", true, MidiFileFormat.SingleTrack,
                new WritingSettings
                {
                    UseRunningStatus = true,
                    NoteOffAsSilentNoteOn = true
                });
        }
        void CreateMidi()
        {
            var midiFile = new MidiFile(new TrackChunk(new SetTempoEvent(500000)),
                new TrackChunk(new TextEvent("It's just single note track..."),
                new NoteOnEvent((SevenBitNumber)60, (SevenBitNumber)45),
                new NoteOffEvent((SevenBitNumber)60, (SevenBitNumber)0)
                {
                    DeltaTime = 400
                }));

            midiFile.Write("My Future Great Song.mid");
        }
    }
}

