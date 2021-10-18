using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Audio;
using System.IO;
using System;

namespace Neat.Audio
{
    public class AudioLoader
    {
        public static void FindAndLoad(string directory, AudioSource source, Action onClipReady)
        {
            Action<string> onSelect = delegate (string file) { Load(source, file, onClipReady); };
            Neat.FileBrowser.FileBrowser.instance.Show(directory, onSelect, ".mp3", ".wav");
        }
        public static void Load(AudioSource source, string filepath, Action onClipReady)
        {
            Action<AudioClip> clipLoaded =
                delegate (AudioClip clip) { SetClip(source, clip, onClipReady); };
            AudioManager.LoadClip(filepath, clipLoaded);
        }
        public static void SetClip(AudioSource source, AudioClip clip, Action onClipReady)
        {
            AudioManager.SetClip(source, clip, onClipReady);
        }
    }
}