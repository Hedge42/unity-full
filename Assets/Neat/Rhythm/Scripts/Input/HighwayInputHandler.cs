using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Music
{
    public class HighwayInputHandler : MonoBehaviour
    {
        private NoteHighway highway;

        //private HighwayHoverHandler _hover;
        //public HighwayHoverHandler hover => _hover == null ? _hover = new HighwayHoverHandler() : _hover;
        public HighwayHoverHandler hover;
        // click handler ??

        private void Awake()
        {
            highway = GetComponent<NoteHighway>();
            hover = new HighwayHoverHandler();
            hover.enabled = true;
            // hover.Enable(enabled);
        }

        private void Update()
        {
            // print(Input.mousePosition);
            hover.GetInput();

            // hover.GetInput();
        }
    }
}
