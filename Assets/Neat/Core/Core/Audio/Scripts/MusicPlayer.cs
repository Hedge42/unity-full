using UnityEngine;

namespace Neat.Audio.MusicPlayer
{
    public class MusicPlayer : MonoBehaviour
    {
        public static MusicPlayer instance;

        private AudioSource source;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                print("Oops!");
                Destroy(gameObject);
            }

            source = GetComponent<AudioSource>();
        }

        public void LoadSong(string path)
        {
            StartCoroutine(AudioManager.instance.GetClip(path, source));
        }
        public void LoadSong(string path, bool playOnLoad)
        {
            if (!playOnLoad)
                LoadSong(path);
            else
                StartCoroutine(AudioManager.instance.GetClip(path, source, PlayPause));
        }

        public void PlayPause()
        {
            if (source.isPlaying)
                source.Pause();
            else
                source.Play();
        }

        public void Skip(float timestamp)
        {
            source.time = timestamp;
        }
    }
}
