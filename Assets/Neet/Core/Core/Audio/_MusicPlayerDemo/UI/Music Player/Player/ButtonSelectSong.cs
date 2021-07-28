using System;
using UnityEngine;
using UnityEngine.UI;
using Neet.File;

namespace Neet.Audio.MusicPlayer
{
    public class ButtonSelectSong : MonoBehaviour
    {
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OpenCustomSongBrowser);
        }

        private void OpenExplorer()
        {
            try
            {
                string songPath = FileManager.OpenAudio("Select Song...");
                if (songPath != "")
                    StartCoroutine(AudioManager.instance.GetClip(songPath, AudioManager.instance.musicSource));
            }
            catch (Exception e)
            {
                print("Oops! " + e.Message);
            }
        }
        private void OpenCustomSongBrowser()
        {
            // CanvasNavigator.instance.Activate(1, 0);
        }
    }
}
