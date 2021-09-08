using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Neat.Music
{
    public class NoteUI : MonoBehaviour
    {
        public UIEventHandler eventHandler;

        public TextMeshProUGUI text;
        public Image background;

        public Note note;
    }
}
