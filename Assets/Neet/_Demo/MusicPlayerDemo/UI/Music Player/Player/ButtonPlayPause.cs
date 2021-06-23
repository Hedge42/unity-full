using UnityEngine;
using UnityEngine.UI;

namespace Neet.Audio.MusicPlayer
{
    public class ButtonPlayPause : MonoBehaviour
    {
        private Button button;
        private AudioSource musicSource;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(PlayPause);
            musicSource = AudioManager.instance.musicSource;
        }

        private void PlayPause()
        {
            if (musicSource.isPlaying)
                musicSource.Pause();
            else
                musicSource.Play();
        }
    }
}
