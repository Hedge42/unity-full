using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Demos.Fighter
{
    public class CharacterSelectorComponent : MonoBehaviour
    {
        public CharacterDatabase db;

        [HideInInspector]
        public Character character;
    }
}
