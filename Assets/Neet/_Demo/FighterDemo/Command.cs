using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Fighter
{
    public class Command
    {
        public string name;
        public int button;
        public string directions;

        public Command(string _name, int _button, string _dir)
        {
            name = _name;
            button = _button;
            directions = _dir;
        }
    }
}
