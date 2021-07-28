using UnityEngine;
using TMPro;

namespace Neet.Audio.MusicPlayer
{
    public class TMPSongTitle : MonoBehaviour
    {
        private TextMeshProUGUI tmp;

        private void Awake()
        {
            tmp = GetComponent<TextMeshProUGUI>();
            AudioManager.onClipLoaded += UpdateTitle;
        }

        void UpdateTitle(AudioManager am)
        {
            if (am?.musicSource?.clip != null)
                tmp.text = "Now playing:\n" + am.musicSource.clip.name;
        }
    }
}
