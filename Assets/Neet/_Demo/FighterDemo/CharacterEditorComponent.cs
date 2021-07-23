using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.File;

namespace Neet.Fighter
{
    [RequireComponent(typeof(GizmoDrawer))]
    public class CharacterEditorComponent : MonoBehaviour
    {
        public CharacterDatabase db;
        public CharacterDatabase characterTemplates;
        public MoveDatabase moveTemplates;

        private GizmoDrawer _drawer;

        public GizmoDrawer drawer
        {
            get
            {
                if (_drawer == null)
                    _drawer = GetComponent<GizmoDrawer>();
                return _drawer;
            }
        }
        public bool active
        {
            get { return drawer.active; }
            set { drawer.active = value; }
        }

        public void SetState(Character c, Move m, int frame)
        {
            drawer.c = c;
            drawer.m = m;
            drawer.frame = frame;
        }
    }
}
