using UnityEngine;

namespace Neat.Audio.Music
{
    public class MusicPlayer_ : MonoBehaviour
    {
        public static MusicPlayer_ instance;

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
