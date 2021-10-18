using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Neat.Audio;

namespace Neat.Music
{
    [ExecuteAlways]
    public class AudioLoaderButton : MonoBehaviour, IMusicPlayerControl
    {
        public MusicPlayer player;
        public Button btn;
        public TextMeshProUGUI tmp;

        private void Awake()
        {
            btn.onClick.AddListener(FindClip);

            player.onEnable.AddListener(Enable);
        }
        private void Start()
        {
            UpdateText();
        }
        public void FindClip()
        {
            player.FindClip(player.musicDirectory);
        }
        public void UpdateText()
        {
            if (player.source.clip == null)
                tmp.text = "No audio clip";
            else if (player.source.clip.loadState != AudioDataLoadState.Loaded)
                tmp.text = "Not ready";
            else
                tmp.text = player.source.clip.name;
        }
        public void Enable(bool value)
        {
            UpdateText();
        }
    }
}
