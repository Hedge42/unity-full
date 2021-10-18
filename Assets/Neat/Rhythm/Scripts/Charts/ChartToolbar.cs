using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Neat.Audio;
using Neat.FileManagement;

namespace Neat.Music
{
    public class ChartToolbar : MonoBehaviour
    {
        private ChartPlayer _controller;
        public ChartPlayer controller
        {
            get
            {
                if (_controller == null)
                    _controller = GetComponent<ChartPlayer>();
                return _controller;
            }
        }

        public AudioLoaderButton loadAudioButton;
    }
}