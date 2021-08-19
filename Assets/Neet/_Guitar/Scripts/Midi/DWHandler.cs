using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Devices;
using UnityEngine;

public class DWMHandle : MonoBehaviour
{



    private Playback _playback;
    private OutputDevice _outputDevice;

    // Start is called before the first frame update
    void Start()
    {
        //var midiFile = MidiFile.Read("Assets/GROOVE.MID");
        //_outputDevice = OutputDevice.GetByName("Microsoft GS Wavetable Synth");
        //_playback = midiFile.GetPlayback(_outputDevice, new MidiClockSettings
        //{
        //    //CreateTickGeneratorCallback = interval => new RegularPrecisionTickGenerator(interval)
        //    //CreateTickGeneratorCallback = interval => new RegularPrecisionTickGenerator()
        //}) ;
        //_playback.InterruptNotesOnStop = true;
        //StartCoroutine(StartMusic());
    }

    private IEnumerator StartMusic()
    {
        _playback.Start();
        while (_playback.IsRunning)
        {
            yield return null;
            _playback.TickClock();
        }
        _playback.Dispose();

    }

    private void OnApplicationQuit()
    {
        _playback.Stop();
        _playback.Dispose();
    }
}