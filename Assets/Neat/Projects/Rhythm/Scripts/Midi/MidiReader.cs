using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Tools;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Multimedia;
using System.Threading;
using System.Collections;
using Note = Melanchall.DryWetMidi.Interaction.Note;

namespace Neat.Audio.Midi
{
    public class MidiReader : MonoBehaviour
    {
        // https://melanchall.github.io/drywetmidi/

        public string path;
        public MidiFile midiFile;
        
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


        public void OpenPath()
        {
            Neat.GameManager.FileManager.OpenExplorer(path);
        }
        public void ReadFile()
        {
            LoadMidi();
            
        } 

        public static void ReadChunks(MidiFile m)
		{
            print(m.GetTrackChunks().Count() + " chunks");
            foreach (TrackChunk t in m.GetTrackChunks())
			{
                print(t.GetNotes().Count);

                // t.
			}
		}

        public ICollection<Note> GetNotesBetween()
		{
            //midiFile.GetNotes().Where(note => note.Time > 0f);
            return midiFile.GetNotes();
            throw new System.NotImplementedException();
		}

        public static void Quantize(MidiFile m, ICollection<Note> notes, MusicalTimeSpan span)
        {
            var tm = m.GetTempoMap();
            var settings = new NotesQuantizingSettings() { FixOppositeEnd = true };
            var grid = new SteppedGrid(MusicalTimeSpan.Eighth);
            new NotesQuantizer().Quantize(notes, grid, tm, settings);
        }

        public static void PrintNotes(MidiFile m)
		{
            var tm = m.GetTempoMap();
            foreach (Note n in m.GetNotes())
                PrintNote(n, tm);
		}

        private static void PrintNote(Note s, TempoMap tm)
        {
            print("note(" + s.NoteNumber + ") "
                    + "metricTime(" + s.TimeAs(TimeSpanType.Metric, tm) + ") "
                    + "metricLength(" + s.LengthAs<MetricTimeSpan>(tm) + ") \n"
                    + "barBeatTime(" + s.TimeAs<BarBeatFractionTimeSpan>(tm) + ") "
                    + "barBeatLength(" + s.LengthAs<BarBeatFractionTimeSpan>(tm) + ") "
                    );
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

