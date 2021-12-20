using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neat.Audio.Music;

[ExecuteAlways]
public class PlayPauseButton : MonoBehaviour, IMusicPlayerControl
{
    public Button btn;
    public Image imgPlay;
    public Image imgPause;
    public MusicPlayer player;


    private void Awake()
    {
        player.onEnable.AddListener(Enable);
        player.onPlayPause.AddListener(UpdateImage);

        btn = GetComponent<Button>();
        btn.onClick.AddListener(Click);
    }

    // IMusicPlayerControl
    public void Enable(bool value)
    {
        btn.interactable = value;
    }

    public void Click()
    {
        if (!player.isPlaying)
            player.Play();
        else
            player.Pause();

        // called by MusicPlayerEvent event
        // UpdateImage(player.isPlaying);
    }

    public void UpdateImage(bool playing)
    {
        imgPlay.enabled = !playing;
        imgPause.enabled = playing;
    }
}
