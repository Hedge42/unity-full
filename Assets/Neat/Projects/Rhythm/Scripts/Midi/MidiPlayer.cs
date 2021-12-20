using Melanchall.DryWetMidi.Core;
using UnityEngine;
using Melanchall.DryWetMidi.Multimedia;
using System.Collections;

namespace Neat.Audio.Midi
{
    public class MidiPlayer : MonoBehaviour
    {
        private Playback playback;
        private OutputDevice output;
        private bool isPlaying;
        private bool hasPlayback;

        private void Start()
        {
            SetErrorListener();
            print("hello");
        }
        private void Update()
        {
            if (hasPlayback && UnityEngine.Input.GetKeyDown(KeyCode.Space))
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
        private Playback GetPlayback(MidiFile midiFile)
        {
            //output = OutputDevice.GetByName("Microsoft GS Wavetable Synth");
            output = OutputDevice.GetByIndex(0);

            playback = midiFile.GetPlayback(output, new PlaybackSettings()
            {
                ClockSettings = new MidiClockSettings()
                {
                    CreateTickGeneratorCallback = () => null
                }
            });

            hasPlayback = true;

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
    }
}